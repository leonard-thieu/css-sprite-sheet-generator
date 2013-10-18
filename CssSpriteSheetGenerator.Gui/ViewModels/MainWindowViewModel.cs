using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using CssSpriteSheetGenerator.Gui.Infrastructure;
using CssSpriteSheetGenerator.Gui.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using _SpriteSheetViewModel = CssSpriteSheetGenerator.Gui.ViewModels.SpriteSheetViewModel;

namespace CssSpriteSheetGenerator.Gui.ViewModels
{
    /// <summary>
    /// The view model for the <see cref="MainWindow" /> class.
    /// </summary>
    public sealed class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        private bool isExiting;

        #region Properties

        /// <summary>
        /// The child view model that handles interaction with <see cref="CssSpriteSheetGenerator.Models.SpriteSheetGenerator" />.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ISpriteSheetViewModel SpriteSheetViewModel { get; set; }

        /// <summary>
        /// Identifies the <see cref="FileName" /> property.
        /// </summary>
        public const string FileNamePropertyName = "FileName";
        private string _FileName;
        /// <summary>
        /// The file name of the sprite sheet.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public string FileName
        {
            get { return _FileName; }
            set
            {
                if (_FileName == value)
                    return;
                var oldValue = _FileName;
                _FileName = value;
                RaisePropertyChanged(FileNamePropertyName, oldValue, value, true);
            }
        }

        #endregion

        #region View Commands

        #region Exit Command

        /// <summary>
        /// Identifies the <see cref="Exit" /> command.
        /// </summary>
        public const string ExitNotification = "Exit";
        private RelayCommand<CancelEventArgs> _Exit;
        /// <summary>
        /// Exits the application.
        /// </summary>
        public RelayCommand<CancelEventArgs> Exit
        {
            get
            {
                return _Exit ?? (_Exit = new RelayRoutedUICommand<CancelEventArgs>(
                    e =>
                    {
                        Action exit = () => { Helper.SendMessage(ExitNotification); };

                        if (!isExiting)
                            SpriteSheetViewModel.PromptSaveExecute(
                                () =>
                                {
                                    isExiting = true;
                                    exit();
                                },
                                () =>
                                {
                                    if (e != null)
                                        e.Cancel = true;
                                });
                        else
                            exit();
                    },
                    Resources.ExitHeader,
                    this.GetInputGestures("Alt+F4")));
            }
        }

        #endregion

        #region Help Command

        /// <summary>
        /// Identifies the <see cref="Help" /> command.
        /// </summary>
        public const string HelpNotification = "Help";
        private RelayCommand _Help;
        /// <summary>
        /// Displays the help file for the application.
        /// </summary>
        public RelayCommand Help
        {
            get
            {
                return _Help ?? (_Help = new RelayRoutedUICommand(
                    () => { Helper.SendMessage(HelpNotification); },
                    () => { return File.Exists(Settings.Default.HelpFile); },
                    Resources.HelpHeader,
                    this.GetInputGestures("F1")));
            }
        }

        #endregion

        #region About Command

        /// <summary>
        /// Identifies the <see cref="About" /> command.
        /// </summary>
        public const string AboutNotification = "About";
        private RelayCommand _About;
        /// <summary>
        /// Displays the About windows.
        /// </summary>
        public RelayCommand About
        {
            get
            {
                return _About ?? (_About = new RelayRoutedUICommand(
                    () =>
                    {
                        Helper.SendMessage(AboutNotification);
                    },
                    Resources.AboutHeader,
                    null));
            }
        }

        #endregion

        #region Options Command

        /// <summary>
        /// Identifies the <see cref="Options" /> command.
        /// </summary>
        public const string OptionsNotification = "Options";
        private RelayCommand _Options;
        /// <summary>
        /// Displays the Options window.
        /// </summary>
        public RelayCommand Options
        {
            get
            {
                return _Options ?? (_Options = new RelayRoutedUICommand(
                    () =>
                    {
                        //Helper.SendMessage(OptionsNotification);
                    },
                    () =>
                    {
                        return false;
                    },
                    Resources.OptionsHeader,
                    this.GetInputGestures("F10")));
            }
        }

        #endregion

        #endregion

        #region Model Commands

        #region New Command

        /// <summary>
        /// Identifies the <see cref="New" /> command.
        /// </summary>
        public const string NewNotification = "New";
        /// <summary>
        /// Creates a new sprite sheet generator.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public RelayCommand New { get { return SpriteSheetViewModel.New; } }

        #endregion

        #region Import Command

        /// <summary>
        /// Identifies the <see cref="Import" /> command.
        /// </summary>
        public const string ImportNotification = "Import";
        /// <summary>
        /// Requests the view to open an open dialog and loads the state of a sprite sheet.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public RelayCommand Import { get { return SpriteSheetViewModel.Import; } }

        #endregion

        #region Export Command

        /// <summary>
        /// Identifies the <see cref="Export" /> command.
        /// </summary>
        public const string ExportNotification = "Export";
        /// <summary>
        /// Requests the view to open a save dialog and saves the state representing the sprite sheet.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public RelayCommand Export { get { return SpriteSheetViewModel.Export; } }

        #endregion

        #region Save Image Command

        /// <summary>
        /// Identifies the <see cref="SaveImage" /> command.
        /// </summary>
        public const string SaveImageNotification = "SaveImage";
        /// <summary>
        /// Requests the view to open a save dialog and saves the image representing the sprite sheet.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public RelayCommand SaveImage { get { return SpriteSheetViewModel.SaveImage; } }

        #endregion

        #region Save CSS Command

        /// <summary>
        /// Identifies the <see cref="SaveCss" /> command.
        /// </summary>
        public const string SaveCssNotification = "SaveCss";
        /// <summary>
        /// Requests the view to open a save dialog and saves the CSS representing the sprite sheet.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public RelayCommand SaveCss { get { return SpriteSheetViewModel.SaveCss; } }

        #endregion

        #region Toggle Select Rectangle Tool Command

        /// <summary>
        /// Identifies the <see cref="ToggleRectangleSelectTool" /> command.
        /// </summary>
        public const string ToggleRectangleSelectToolNotification = "ToggleRectangleSelectTool";
        /// <summary>
        /// Toggles the state of the Rectangle Select tool.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public RelayCommand ToggleRectangleSelectTool { get { return SpriteSheetViewModel.ToggleRectangleSelectTool; } }

        #endregion

        #region Cut Command

        /// <summary>
        /// Identifies the <see cref="Cut" /> command.
        /// </summary>
        public const string CutNotification = "Cut";
        /// <summary>
        /// Performs a cut operation on the selected objects.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public RelayCommand Cut { get { return SpriteSheetViewModel.Cut; } }

        #endregion

        #region Debug Command

        /// <summary>
        /// Identifies the <see cref="Debug" /> command.
        /// </summary>
        public const string DebugNotification = "Debug";
        /// <summary>
        /// Causes the debugger to break.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public RelayCommand Debug { get { return SpriteSheetViewModel.DebugCommand; } }

        #endregion

        #region Add Sample Image Command

        /// <summary>
        /// Identifies the <see cref="AddSampleImage" /> command.
        /// </summary>
        public const string AddSampleImageNotification = "AddSampleImage";
        /// <summary>
        /// Adds a sample image.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public RelayCommand AddSampleImage { get { return SpriteSheetViewModel.AddSampleImageCommand; } }

        #endregion

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainWindowViewModel(ISpriteSheetViewModel spriteSheetViewModel)
        {
            SpriteSheetViewModel = spriteSheetViewModel;
            SpriteSheetViewModel.PropertyChanged += new PropertyChangedEventHandler(OnSpriteSheetViewModelPropertyChanged);
            FileName = SpriteSheetViewModel.FileName;
        }

        // Updates FileName when the corresponding property is changed on SpriteSheetViewModel
        private void OnSpriteSheetViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _SpriteSheetViewModel.FileNamePropertyName)
            {
                var spriteSheetViewModel = sender as ISpriteSheetViewModel;
                FileName = spriteSheetViewModel.FileName;
            }
        }
    }
}