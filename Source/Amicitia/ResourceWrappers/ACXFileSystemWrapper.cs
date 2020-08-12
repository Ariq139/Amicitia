﻿using System.IO;
using System.Windows.Forms;
using Amicitia.Utilities;
using AmicitiaLibrary.IO;
using AtlusFileSystemLibrary.FileSystems.ACX;

namespace Amicitia.ResourceWrappers
{
    public class ACXFileSystemWrapper : ResourceWrapper<ACXFileSystem>
    {
        public ACXFileSystemWrapper( string text, ACXFileSystem resource ) : base( text, resource )
        {
        }

        protected override void Initialize()
        {
            CommonContextMenuOptions = CommonContextMenuOptions.Export | CommonContextMenuOptions.Replace | CommonContextMenuOptions.Add |
                                       CommonContextMenuOptions.Move | CommonContextMenuOptions.Rename | CommonContextMenuOptions.Delete;

            RegisterFileExportAction( SupportedFileType.PakArchiveFile, ( res, path ) =>
            {
                res.Save().SaveToFile( leaveOpen: false, path );

            } );
            RegisterFileReplaceAction( SupportedFileType.PakArchiveFile, ( res, path ) =>
            {
                var pak = new ACXFileSystem();
                pak.Load( File.OpenRead( path ).ToMemoryStream( leaveOpen: false ), true );
                return pak;
            } );
            RegisterFileAddAction( SupportedFileType.Resource, DefaultFileAddAction );
            RegisterRebuildAction( ( wrap ) =>
            {
                var file = new ACXFileSystem();

                foreach ( IResourceWrapper entry in Nodes )
                {
                    file.AddFile( entry.GetResourceMemoryStream() );
                }

                return file;
            } );
        }

        protected override void PopulateView()
        {
            foreach ( int file in Resource.EnumerateFiles() )
            {
                Nodes.Add( ( TreeNode )ResourceWrapperFactory.GetResourceWrapper( $"Sound{file:D2}.adx", Resource.OpenFile(file) ) );
            }
        }
    }
}
