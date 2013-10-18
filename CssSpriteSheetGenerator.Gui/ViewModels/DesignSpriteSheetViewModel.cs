using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using CssSpriteSheetGenerator.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CssSpriteSheetGenerator.Gui.ViewModels
{
#pragma warning disable 1591
    [ExcludeFromCodeCoverage]
    public sealed class DesignSpriteSheetViewModel : ViewModelBase, ISpriteSheetViewModel
    {
        private readonly CompositeCollection _Children;

        #region Properties

        public bool IsEmpty { get; set; }
        public string FileName { get; set; }
        public bool IsModified { get; set; }
        public CompositeCollection Children { get { return _Children; } }
        public int HorizontalOffset { get; set; }
        public int VerticalOffset { get; set; }
        public IEnumerable<Arrange> BuildMethods { get { return Enum.GetValues(typeof(Arrange)).Cast<Arrange>(); } }
        public Arrange BuildMethod { get; set; }
        public SpriteSheetGenerator SpriteSheetGenerator { get; set; }
        public bool IsRectangleSelectToolEnabled { get; set; }

        #endregion

        #region Commands

        public RelayCommand New { get; set; }
        public RelayCommand Import { get; set; }
        public RelayCommand Export { get; set; }
        public RelayCommand SaveImage { get; set; }
        public RelayCommand SaveCss { get; set; }
        public RelayCommand ToggleRectangleSelectTool { get; set; }
        public RelayCommand Cut { get; set; }
        public RelayCommand Build { get; set; }
        public RelayCommand DebugCommand { get; set; }
        public RelayCommand AddSampleImageCommand { get; set; }

        #endregion

        public DesignSpriteSheetViewModel()
        {
            SpriteSheetGenerator = new SpriteSheetGenerator();
            _Children = new CompositeCollection
            {
                new CollectionContainer { Collection = SpriteSheetGenerator.SpriteSheets },
                new CollectionContainer { Collection = SpriteSheetGenerator.Sprites }
            };

            BuildMethod = Arrange.Optimal;
            HorizontalOffset = 1;
            VerticalOffset = 1;

            AddSampleImage(SpriteSheetGenerator);
        }

        public static void AddSampleImage(SpriteSheetGenerator spriteSheetGenerator)
        {
            if (spriteSheetGenerator == null)
                throw new ArgumentNullException("spriteSheetGenerator");

            try
            {
                string fileName;
                if (IsInDesignModeStatic)
                    fileName = @"S:\Projects\CssSpriteSheetGenerator\Data\Close.png";
                else
                    fileName = @"..\Data\Close.png";
                var bounds = new Rect(50, 50, 16, 16).ToDrawingRectangle();
                spriteSheetGenerator.AddSpriteSheet(fileName, 50, 50);
                spriteSheetGenerator.AddSprite("ExampleSprite", bounds);
            }
            catch (ArgumentException ex)
            {
                ex.LogAndSendExceptionNotification();
            }
        }

        #region Public Methods

        public SpriteSheet AddSpriteSheet(string fileName, Point position)
        {
            throw new NotImplementedException();
        }

        public void RemoveSpriteSheet(string className)
        {
            throw new NotImplementedException();
        }

        public void AddSprite(string className, Rect bounds)
        {
            throw new NotImplementedException();
        }

        public void RemoveSprite(string className)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
#pragma warning restore 1591
}
