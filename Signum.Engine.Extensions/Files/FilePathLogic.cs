﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Engine.Maps;
using Signum.Entities.Files;
using Signum.Entities;
using Signum.Engine.Basics;
using Signum.Utilities;
using System.IO;
using Signum.Engine.Extensions.Properties;
using Signum.Engine.DynamicQuery;
using System.Reflection;
using System.Diagnostics;

namespace Signum.Engine.Files
{

    public static class FilePathLogic
    {
        static Dictionary<Enum, FileTypeAlgorithm> fileTypes = new Dictionary<Enum, FileTypeAlgorithm>();

        public static void Start(SchemaBuilder sb, DynamicQueryManager dqm)
        {
            if (sb.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                sb.Include<FilePathDN>();

                EnumLogic<FileTypeDN>.Start(sb, () => fileTypes.Keys.ToHashSet());

                sb.Schema.EntityEvents<FilePathDN>().PreSaving += FilePath_PreSaving;
                sb.Schema.EntityEvents<FilePathDN>().Deleting += FilePath_Deleting;

                dqm[typeof(FileRepositoryDN)] = (from r in Database.Query<FileRepositoryDN>()
                                                 select new
                                                 {
                                                     Entity = r.ToLite(),
                                                     r.Id,
                                                     r.Name,
                                                     r.Active,
                                                     r.PhysicalPrefix,
                                                     r.WebPrefix
                                                 }).ToDynamic();

                dqm[typeof(FilePathDN)] = (from p in Database.Query<FilePathDN>()
                                           select new
                                           {
                                               Entity = p.ToLite(),
                                               p.Id,
                                               p.FileName,
                                               FileType = p.FileType.ToLite(),
                                               p.FullPhysicalPath,
                                               p.FullWebPath,
                                               Repository = p.Repository.ToLite()
                                           }).ToDynamic();

                dqm[typeof(FileTypeDN)] = (from f in Database.Query<FileTypeDN>()
                                           select new
                                           {
                                               Entity = f.ToLite(),
                                               f.Name
                                           }).ToDynamic();

                sb.Schema.Generating += new Func<SqlPreCommand>(Schema_Generating);
            }
        }

        static SqlPreCommand Schema_Generating()
        {
            return Administrator.AddMultiColumnIndexScript<FilePathDN>(true, f => f.Sufix, f => f.Repository);
        }

        static void FilePath_Deleting(Type type, int id)
        {
            string fullPath = (from f in Database.Query<FilePathDN>()
                               where f.Id == id
                               select f.FullPhysicalPath).Single();

            Transaction.RealCommit += () =>
            {
                if (unsafeMode)
                    Debug.WriteLine(fullPath);
                else
                    File.Delete(fullPath);
            };
        }

        

        const long ERROR_DISK_FULL = 112L; // see winerror.h

        [ThreadStatic]
        static bool unsafeMode = false;

        public static IDisposable UnsafeMode()
        {
            unsafeMode = true;
            return new Disposable(() => unsafeMode = false);
        }

        public static FilePathDN UnsafeLoad(FileRepositoryDN repository, FileTypeDN fileType, string fullPath)
        {
            if (!fullPath.StartsWith(repository.FullPhysicalPrefix))
                throw new InvalidOperationException("The File {0} doesn't belong to the repository {1}".Formato(fullPath, repository.PhysicalPrefix));

            return new FilePathDN
            {
                FileLength = (int)new FileInfo(fullPath).Length,
                FileType = fileType,
                Sufix = fullPath.Substring(repository.FullPhysicalPrefix.Length).TrimStart('\\'),
                FileName = Path.GetFileName(fullPath),
                Repository = repository,
            };
        }

        static void FilePath_PreSaving(FilePathDN fp, bool isRoot, ref bool graphModified)
        {
            if (fp.IsNew && !unsafeMode)
            {
                //set typedn from enum
                if (fp.FileType == null)
                    fp.FileType = EnumLogic<FileTypeDN>.ToEntity(fp.FileTypeEnum);

                //set enum from typedn
                if (fp.FileTypeEnum == null)
                    fp.SetFileTypeEnum(EnumLogic<FileTypeDN>.ToEnum(fp.FileType));

                FileTypeAlgorithm alg = fileTypes[fp.FileTypeEnum];
                string sufix = alg.CalculateSufix(fp);
                if (!sufix.HasText())
                    throw new ApplicationException(Resources.SufixNotSet);

                do
                {
                    fp.Repository = alg.GetRepository(fp);
                    if (fp.Repository == null)
                        throw new ApplicationException(Resources.RepositoryNotSet);
                    int i = 2;
                    fp.Sufix = sufix;
                    while (File.Exists(fp.FullPhysicalPath) && alg.RenameOnCollision)
                    {
                        fp.Sufix = alg.RenameAlgorithm(sufix, i);
                        i++;
                    }
                }
                while (!SaveFile(fp));
            }
        }

        private static bool SaveFile(FilePathDN fp)
        {
            try
            {
                string path = Path.GetDirectoryName(fp.FullPhysicalPath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                File.WriteAllBytes(fp.FullPhysicalPath, fp.BinaryFile);
            }
            catch (IOException ex)
            {
                int hresult = (int)ex.GetType().GetField("_HResult",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(ex); // The error code is stored in just the lower 16 bits
                if ((hresult & 0xFFFF) == ERROR_DISK_FULL)
                {
                    fp.Repository.Active = false;
                    Database.Save(fp.Repository);
                    return false;
                }
                else
                    throw;
            }
            return true;
        }

        public static void Register(Enum fileTypeKey, FileTypeAlgorithm algorithm)
        {
            fileTypes.Add(fileTypeKey, algorithm);
        }

        public static byte[] GetByteArray(FilePathDN fp)
        {
            return File.ReadAllBytes(fp.FullPhysicalPath);
        }
    }

    public sealed class FileTypeAlgorithm
    {
        public Func<FilePathDN, FileRepositoryDN> GetRepository { get; set; }
        public Func<FilePathDN, string> CalculateSufix { get; set; }
        
        bool renameOnCollision = true;
        public bool RenameOnCollision
        {
            get { return renameOnCollision; }
            set { renameOnCollision = value; }
        }

        public Func<string, int, string> RenameAlgorithm { get; set; }

        public FileTypeAlgorithm()
        {
            RenameAlgorithm = DefaultRenameAlgorithm;
            GetRepository = DefaultGetRepository;
            CalculateSufix = DefaultCalculateSufix;
        }

        public static readonly Func<string, int, string> DefaultRenameAlgorithm = (sufix, num) =>
           Path.Combine(Path.GetDirectoryName(sufix),
              "{0}({1}){2}".Formato(Path.GetFileNameWithoutExtension(sufix), num, Path.GetExtension(sufix)));

        public static readonly Func<FilePathDN, FileRepositoryDN> DefaultGetRepository = (FilePathDN fp) =>
            Database.Query<FileRepositoryDN>().FirstOrDefault(r => r.Active && r.FileTypes.Contains(fp.FileType));

        public static readonly Func<FilePathDN, string> DefaultCalculateSufix = (FilePathDN fp) =>
            Path.Combine(TimeZoneManager.Now.ToString("yyyy-MM-dd"), fp.FileName);
    }
}
