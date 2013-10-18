using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using CssSpriteSheetGenerator.Gui.Infrastructure;
using CssSpriteSheetGenerator.Gui.Properties;
using CssSpriteSheetGenerator.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CssSpriteSheetGenerator.Gui.ViewModels
{
    /// <summary>
    /// The view model that handles interaction with <see cref="SpriteSheetGenerator" />.
    /// </summary>
    public sealed class SpriteSheetViewModel : ViewModelBase, ISpriteSheetViewModel
    {
        private CompositeCollection _Children;

        #region Properties

        /// <summary>
        /// Identifies the <see cref="IsEmpty" /> property.
        /// </summary>
        public const string IsEmptyPropertyName = "IsEmpty";
        private bool _IsEmpty;
        /// <summary>
        /// Indicates if there are any images on the sprite sheet.
        /// </summary>
        public bool IsEmpty
        {
            get { return _IsEmpty; }
            set
            {
                if (_IsEmpty == value)
                    return;
                var oldValue = _IsEmpty;
                _IsEmpty = value;
                RaisePropertyChanged(IsEmptyPropertyName, oldValue, value, true);
            }
        }

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
            get { return _FileName ?? Resources.Untitled; }
            set
            {
                if (_FileName == value)
                    return;
                var oldValue = _FileName;
                _FileName = value;
                RaisePropertyChanged(FileNamePropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// A flattened collection of all the sprite sheets and sprites.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public CompositeCollection Children
        {
            get
            {
                if (_Children == null)
                {
                    _Children = new CompositeCollection();
                    ((INotifyCollectionChanged)_Children).CollectionChanged += new NotifyCollectionChangedEventHandler(OnChildrenCollectionChanged);
                }

                return _Children;
            }
        }

        /// <summary>
        /// The members of <see cref="Arrange" />.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public IEnumerable<Arrange> BuildMethods { get { return Enum.GetValues(typeof(Arrange)).Cast<Arrange>(); } }

        /// <summary>
        /// Identifies the <see cref="IsModified" /> property.
        /// </summary>
        public const string IsModifiedPropertyName = "IsModified";
        private bool _IsModified;
        /// <summary>
        /// Indicates if the sprite sheet generator has been modified since it was last saved or loaded.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool IsModified
        {
            get { return _IsModified; }
            set
            {
                if (_IsModified == value)
                    return;
                var oldValue = _IsModified;
                _IsModified = value;
                RaisePropertyChanged(IsModifiedPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// Identifies the <see cref="BuildMethod" /> property.
        /// </summary>
        public const string BuildMethodPropertyName = "BuildMethod";
        /// <summary>
        /// Specifies how sprites are arranged.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public Arrange BuildMethod
        {
            get { return SpriteSheetGenerator.BuildMethod; }
            set
            {
                if (SpriteSheetGenerator.BuildMethod == value)
                    return;
                var oldValue = SpriteSheetGenerator.BuildMethod;
                SpriteSheetGenerator.BuildMethod = value;
                RaisePropertyChanged(BuildMethodPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// Identifies the <see cref="HorizontalOffset" /> property.
        /// </summary>
        public const string HorizontalOffsetPropertyName = "HorizontalOffset";
        /// <summary>
        /// The number of pixels that separates each sprite on the horizontal axis.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public int HorizontalOffset
        {
            get { return SpriteSheetGenerator.HorizontalOffset; }
            set
            {
                if (SpriteSheetGenerator.HorizontalOffset == value)
                    return;
                var oldValue = SpriteSheetGenerator.HorizontalOffset;
                SpriteSheetGenerator.HorizontalOffset = value;
                RaisePropertyChanged(HorizontalOffsetPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// Identifies the <see cref="VerticalOffset" /> property.
        /// </summary>
        public const string VerticalOffsetPropertyName = "VerticalOffset";
        /// <summary>
        /// The number of pixels that separates each sprite on the vertical axis.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public int VerticalOffset
        {
            get { return SpriteSheetGenerator.VerticalOffset; }
            set
            {
                if (SpriteSheetGenerator.VerticalOffset == value)
                    return;
                var oldValue = SpriteSheetGenerator.VerticalOffset;
                SpriteSheetGenerator.VerticalOffset = value;
                RaisePropertyChanged(VerticalOffsetPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// Identifies the <see cref="SpriteSheetGenerator" /> property.
        /// </summary>
        public const string SpriteSheetGeneratorPropertyName = "SpriteSheetGenerator";
        private SpriteSheetGenerator _SpriteSheetGenerator;
        /// <summary>
        /// The main class used to generate a sprite sheet.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public SpriteSheetGenerator SpriteSheetGenerator
        {
            get { return _SpriteSheetGenerator; }
            set
            {
                if (_SpriteSheetGenerator == value)
                    return;
                var oldValue = _SpriteSheetGenerator;
                _SpriteSheetGenerator = value;
                RaisePropertyChanged(SpriteSheetGeneratorPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// Identifies the <see cref="IsRectangleSelectToolEnabled" /> property.
        /// </summary>
        public const string IsRectangleSelectToolEnabledPropertyName = "IsRectangleSelectToolEnabled";
        private bool _IsRectangleSelectToolEnabled;
        /// <summary>
        /// If sprite selection mode is currently enabled.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool IsRectangleSelectToolEnabled
        {
            get { return _IsRectangleSelectToolEnabled; }
            set
            {
                if (_IsRectangleSelectToolEnabled == value)
                    return;
                var oldValue = _IsRectangleSelectToolEnabled;
                _IsRectangleSelectToolEnabled = value;
                RaisePropertyChanged(IsRectangleSelectToolEnabledPropertyName, oldValue, value, true);
            }
        }

        #endregion

        #region Commands

        #region New Command

        /// <summary>
        /// Identifies the <see cref="New" /> command.
        /// </summary>
        public const string NewNotification = "New";
        private RelayCommand _New;
        /// <summary>
        /// Creates a new sprite sheet generator.
        /// </summary>
        public RelayCommand New
        {
            get
            {
                return _New ?? (_New = new RelayRoutedUICommand(
                    () =>
                    {
                        this.PromptSaveExecute(() =>
                        {
                            if (SpriteSheetGenerator != null)
                                SpriteSheetGenerator.Dispose();
                            SpriteSheetGenerator = new SpriteSheetGenerator();
                            FileName = null;
                            IsModified = false;
                        });
                    },
                    Resources.NewHeader,
                    this.GetInputGestures("Ctrl+N")));
            }
        }

        #endregion

        #region Import Command

        /// <summary>
        /// Identifies the <see cref="Import" /> command.
        /// </summary>
        public const string ImportNotification = "Import";
        private RelayCommand _Import;
        /// <summary>
        /// Loads a previously saved state of a sprite sheet.
        /// </summary>
        public RelayCommand Import
        {
            get
            {
                return _Import ?? (_Import = new RelayRoutedUICommand(
                    () =>
                    {
                        this.PromptSaveExecute(() =>
                        {
                            Helper.SendMessage<string>(ImportNotification,
                                fileName =>
                                {
                                    try
                                    {
                                        var spriteSheetGenerator = SpriteSheetGenerator.Load(fileName);
                                        if (SpriteSheetGenerator != null)
                                            SpriteSheetGenerator.Dispose();
                                        SpriteSheetGenerator = spriteSheetGenerator;
                                        FileName = Path.GetFileName(fileName);
                                        IsModified = false;
                                    }
                                    catch (InvalidOperationException ex)
                                    {
                                        ex.LogAndSendExceptionNotification();
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.Log();
                                        throw;
                                    }
                                });
                        });
                    },
                    Resources.ImportHeader,
                    this.GetInputGestures("Ctrl+O")));
            }
        }

        #endregion

        #region Export Command

        /// <summary>
        /// Identifies the <see cref="Export" /> command.
        /// </summary>
        public const string ExportNotification = "Export";
        private RelayCommand _Export;
        /// <summary>
        /// Requests the view to open a save dialog and saves the state representing the sprite sheet.
        /// </summary>
        public RelayCommand Export
        {
            get
            {
                return _Export ?? (_Export = new RelayRoutedUICommand(
                    () =>
                    {
                        Helper.SendMessage<string>(ExportNotification,
                            fileName =>
                            {
                                try
                                {
                                    SpriteSheetGenerator.Save(fileName);
                                    FileName = Path.GetFileName(fileName);
                                    IsModified = false;
                                }
                                catch (ArgumentNullException ex)
                                {
                                    ex.LogAndSendExceptionNotification();
                                }
                                catch (Exception ex)
                                {
                                    ex.Log();
                                    throw;
                                }
                            });
                    },
                    () =>
                    {
                        if (SpriteSheetGenerator.SpriteSheets.Count > 0)
                            return true;
                        return false;
                    },
                    Resources.ExportHeader,
                    this.GetInputGestures("Ctrl+S")));
            }
        }

        #endregion

        #region Save Image Command

        /// <summary>
        /// Identifies the <see cref="SaveImage" /> command.
        /// </summary>
        public const string SaveImageNotification = "SaveImage";
        private RelayCommand _SaveImage;
        /// <summary>
        /// Requests the view to open a save dialog and saves the image representing the sprite sheet.
        /// </summary>
        public RelayCommand SaveImage
        {
            get
            {
                return _SaveImage ?? (_SaveImage = new RelayRoutedUICommand(
                    () =>
                    {
                        Helper.SendMessage<string>(SaveImageNotification,
                            fileName =>
                            {
                                try
                                {
                                    SpriteSheetGenerator.SaveImage(fileName);
                                }
                                catch (ArgumentOutOfRangeException ex)
                                {
                                    ex.LogAndSendExceptionNotification();
                                }
                                catch (Exception ex)
                                {
                                    ex.Log();
                                    throw;
                                }
                            });
                    },
                    () =>
                    {
                        if (SpriteSheetGenerator.SpriteSheets.Count > 0)
                            return true;
                        return false;
                    },
                    Resources.SaveImageHeader,
                    this.GetInputGestures("Ctrl+Shift+A")));
            }
        }

        #endregion

        #region Save Css Command

        /// <summary>
        /// Identifies the <see cref="SaveCss" /> command.
        /// </summary>
        public const string SaveCssNotification = "SaveCss";
        private RelayCommand _SaveCss;
        /// <summary>
        /// Requests the view to open a save dialog and saves the CSS representing the sprite sheet.
        /// </summary>
        public RelayCommand SaveCss
        {
            get
            {
                return _SaveCss ?? (_SaveCss = new RelayRoutedUICommand(
                    () =>
                    {
                        Helper.SendMessage<string>(SaveCssNotification,
                               fileName =>
                               {
                                   SpriteSheetGenerator.SaveCss(fileName);
                               });
                    },
                    () =>
                    {
                        if (SpriteSheetGenerator.SpriteSheets.Count > 0)
                            return true;
                        return false;
                    },
                    Resources.SaveCssHeader,
                    this.GetInputGestures("Ctrl+Shift+C")));
            }
        }

        #endregion

        #region Toggle Rectangle Select Tool Command

        /// <summary>
        /// Identifies the <see cref="ToggleRectangleSelectTool" /> command.
        /// </summary>
        public const string ToggleRectangleSelectToolNotification = "ToggleRectangleSelectTool";
        private RelayCommand _ToggleRectangleSelectTool;
        /// <summary>
        /// Enables or disables the Rectangle Select tool.
        /// </summary>
        public RelayCommand ToggleRectangleSelectTool
        {
            get
            {
                return _ToggleRectangleSelectTool ?? (_ToggleRectangleSelectTool = new RelayRoutedUICommand(
                    () => { IsRectangleSelectToolEnabled = IsRectangleSelectToolEnabled ? false : true; },
                    Resources.ToggleRectangleSelectToolHeader,
                    this.GetInputGestures("Ctrl+1")));
            }
        }

        #endregion

        #region Cut Command

        /// <summary>
        /// Identifies the <see cref="Cut" /> command.
        /// </summary>
        public const string CutNotification = "Cut";
        private RelayCommand _Cut;
        /// <summary>
        /// Performs a cut operation on the selected objects.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public RelayCommand Cut
        {
            get
            {
                return _Cut ?? (_Cut = new RelayRoutedUICommand(
                    () => { },
                    () => { return false; },
                    Resources.CutHeader,
                    this.GetInputGestures("Ctrl+X")));
            }
        }

        #endregion

        #region Build Command

        /// <summary>
        /// Identifies the <see cref="Build" /> command.
        /// </summary>
        public const string BuildNotification = "Build";
        private RelayCommand _Build;
        /// <summary>
        /// Builds the sprite sheet.
        /// </summary>
        public RelayCommand Build
        {
            get
            {
                return _Build ?? (_Build = new RelayRoutedUICommand(
                    () => { SpriteSheetGenerator.Build(); },
                    () =>
                    {
                        if (SpriteSheetGenerator.SpriteSheets.Count == 0)
                            return false;
                        if (SpriteSheetGenerator.Mapper == null)
                            return false;
                        return true;
                    },
                    Resources.BuildHeader,
                    this.GetInputGestures("F6")));
            }
        }

        #endregion

        #region Debug Command

        /// <summary>
        /// Identifies the <see cref="DebugCommand" /> command.
        /// </summary>
        public const string DebugCommandNotification = "Debug";
        private RelayCommand _DebugCommand;
        /// <summary>
        /// Causes the debugger to break.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public RelayCommand DebugCommand
        {
            get
            {
                return _DebugCommand ?? (_DebugCommand = new RelayRoutedUICommand(
                    Break(),
                    Resources.DebugHeader,
                    this.GetInputGestures("Ctrl+D")));
            }
        }

        #endregion

        #region Add Sample Image Command

        /// <summary>
        /// Identifies the <see cref="AddSampleImageCommand" /> command.
        /// </summary>
        public const string AddSampleImageCommandNotification = "AddSampleImageCommand";
        private RelayCommand _AddSampleImageCommand;
        /// <summary>
        /// Adds a sample image.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public RelayCommand AddSampleImageCommand { get { return _AddSampleImageCommand ?? (_AddSampleImageCommand = new RelayCommand(new Action(AddSampleImage))); } }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="SpriteSheetViewModel" /> class.
        /// </summary>
        public SpriteSheetViewModel()
        {
            PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
            SpriteSheetGenerator = new SpriteSheetGenerator();

            BuildMethod = Arrange.Optimal;
            HorizontalOffset = 1;
            VerticalOffset = 1;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a sprite sheet to this container sprite sheet.
        /// </summary>
        /// <param name="fileName">The path to the image that will serve as the background image.</param>
        /// <param name="position">The position of the sprite sheet to be added within this container sprite sheet.</param>
        /// <returns>The sprite sheet added to this container sprite sheet.</returns>
        public SpriteSheet AddSpriteSheet(string fileName, Point position)
        {
            var spriteSheet = SpriteSheetGenerator.AddSpriteSheet(fileName, position.ToDrawingPoint());
            var className = Path.GetFileNameWithoutExtension(fileName);
            AddSprite(spriteSheet, className);

            SaveImage.RaiseCanExecuteChanged();
            Build.RaiseCanExecuteChanged();

            return spriteSheet;
        }

        /// <summary>
        /// Removes a sprite sheet from this container sprite sheet.
        /// </summary>
        /// <param name="className">The CSS class that represents the sprite sheet.</param>
        public void RemoveSpriteSheet(string className)
        {
            SpriteSheetGenerator.RemoveSpriteSheet(className);
        }

        /// <summary>
        /// Adds a sprite to a sprite sheet.
        /// </summary>
        /// <param name="parent">The sprite sheet to add the sprite to.</param>
        /// <param name="className">The class name of the sprite.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parent" /> cannot be null.</exception>
        public void AddSprite(SpriteSheet parent, string className)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            AddSprite(parent, className, 0, 0, parent.Image.Width, parent.Image.Height);
        }

        /// <summary>
        /// Adds a sprite to a sprite sheet.
        /// </summary>
        /// <param name="parent">The sprite sheet to add the sprite to.</param>
        /// <param name="className">The class name of the sprite.</param>
        /// <param name="width">The width of the sprite.</param>
        /// <param name="height">The height of the sprite.</param>
        public void AddSprite(SpriteSheet parent, string className, int width, int height)
        {
            AddSprite(parent, className, 0, 0, width, height);
        }

        /// <summary>
        /// Adds a sprite to a sprite sheet.
        /// </summary>
        /// <param name="parent">The sprite sheet to add the sprite to.</param>
        /// <param name="className">The class name of the sprite.</param>
        /// <param name="x">The x-coordinate of the sprite.</param>
        /// <param name="y">The y-coordinate of the sprite.</param>
        /// <param name="width">The width of the sprite.</param>
        /// <param name="height">The height of the sprite.</param>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y")]
        public void AddSprite(SpriteSheet parent, string className, int x, int y, int width, int height)
        {
            var sprite = new Sprite(className, x, y, width, height);
            AddSprite(parent, sprite);
        }

        /// <summary>
        /// Adds a sprite to a sprite sheet.
        /// </summary>
        /// <param name="parent">The sprite sheet to add the sprite to.</param>
        /// <param name="sprite">The sprite to add.</param>
        public void AddSprite(SpriteSheet parent, Sprite sprite)
        {
            SpriteSheetGenerator.AddSprite(parent, sprite);
        }

        /// <summary>
        /// Adds a sprite.
        /// </summary>
        /// <param name="className">The CSS class of the sprite.</param>
        /// <param name="bounds">The position and bounds of the sprite.</param>
        public void AddSprite(string className, Rect bounds)
        {
            SpriteSheetGenerator.AddSprite(className, bounds.ToDrawingRectangle());
        }

        /// <summary>
        /// Removes a sprite.
        /// </summary>
        /// <param name="className">The CSS class of the sprite to remove.</param>
        public void RemoveSprite(string className)
        {
            SpriteSheetGenerator.RemoveSprite(className);
        }

        /// <summary>
        /// Returns the <see cref="Sprite" /> or <see cref="SpriteSheet" /> that matches <paramref name="className" />.
        /// </summary>
        /// <param name="className">The class name to search for.</param>
        /// <returns>
        ///     <para>The <see cref="Sprite" /> or <see cref="SpriteSheet" /> that matches <paramref name="className" />.</para>
        ///     <para>Returns null if a match cannot be found.</para>
        /// </returns>
        public T Find<T>(string className) where T : SpriteBase
        {
            return SpriteSheetGenerator.Find<T>(className);
        }

        #endregion

        #region Private Methods

        // Called when SpriteSheetGenerator is changed
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SpriteSheetGenerator == null)
                return;

            if (e.PropertyName == SpriteSheetGeneratorPropertyName)
            {
                Children.Clear();
                Children.Add(new CollectionContainer { Collection = SpriteSheetGenerator.SpriteSheets });
                Children.Add(new CollectionContainer { Collection = SpriteSheetGenerator.Sprites });
            }
        }

        // Adds event handlers when SpriteSheetGenerator is changed
        private void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (INotifyCollectionChanged collection in e.NewItems)
                    collection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnChildCollectionChanged);
        }

        // Sets IsModified to true
        // Adds PropertyChanged event handlers when a child collection is added
        private void OnChildCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
                IsModified = true;

            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (SpriteBase spriteBase in e.NewItems)
                    spriteBase.PropertyChanged += new PropertyChangedEventHandler(OnChildPropertyChanged);

            if (SpriteSheetGenerator.SpriteSheets.Count == 0)
                IsEmpty = true;
            else
                IsEmpty = false;
        }

        // Sets IsModified to true when a child property is modified
        private void OnChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsModified)
                return;

            IsModified = true;
        }

        // Debugging aid
        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private Action Break()
        {
            return new Action(Debugger.Break);
        }

        // Designtime aid
        [ExcludeFromCodeCoverage]
        private void AddSampleImage()
        {
            DesignSpriteSheetViewModel.AddSampleImage(SpriteSheetGenerator);
        }

        #endregion
    }
}
