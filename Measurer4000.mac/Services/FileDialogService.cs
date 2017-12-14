﻿using System;
using System.Collections.Generic;
using System.Text;
using Measurer4000.Core.Services.Interfaces;
using Foundation;
using AppKit;
using Measurer4000.Core.Models;

namespace Measurer4000.mac.Services
{
    class FileDialogService : IDialogService
    {
        public void OpenFileDialog(Action<string> onFileDialogSuccess, Action<Exception> onFileDialogError) {
            try
            {
                var fileDialog = new NSOpenPanel()
                {
                    Title = "Choose your .sln file",
                    ShowsResizeIndicator = true,
                    ShowsHiddenFiles = true,
                    CanChooseFiles = true,
                    CanCreateDirectories = false,
                    AllowsMultipleSelection = false,
                    AllowedFileTypes = new string[] { "sln" }
                };
                if (fileDialog.RunModal() == (int)NSModalResponse.OK)
                {
                    onFileDialogSuccess?.Invoke(fileDialog.Url.Path);
                }
            }
            catch(Exception e)
            {
				CreateDialog(EnumTypeDialog.Error, e.Message);
            }
        }
		public void CreateDialog(EnumTypeDialog type, string text, string title = "")
		{
            var alert = new NSAlert()
            {
                AlertStyle = (NSAlertStyle)type,
                InformativeText = text,
                MessageText = title
            };
			alert.RunModal();
		}
    }
}
