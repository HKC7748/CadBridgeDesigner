using System;
using Autodesk.AutoCAD.DatabaseServices;
using CadBridgeDesigner.Tool.Bridge.Base;

namespace CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier
{
    public sealed class ColumnPierModel:BridgeComponentModel
    {
        // 盖梁参数
        private double _lengthOfPierCap;
        public double LengthOfPierCap
        {
            get => _lengthOfPierCap;
            set
            {
                _lengthOfPierCap = value;
                OnPropertyChanged();
            }
        }
        private double _widthOfPierCap;
        public double WidthOfPierCap
        {
            get => _widthOfPierCap;
            set
            {
                _widthOfPierCap = value;
                OnPropertyChanged();
            }
        }
        private double _heightOfPierCap;
        public double HeightOfPierCap
        {
            get => _heightOfPierCap;
            set
            {
                _heightOfPierCap = value;
                OnPropertyChanged();
            }
        }
        private double _lengthOfPierCapSlope;
        public double LengthOfPierCapSlope
        {
            get => _lengthOfPierCapSlope;
            set
            {
                _lengthOfPierCapSlope = value;
                OnPropertyChanged();
            }
        }
        private double _heightOfPierCapSlope;
        public double HeightOfPierCapSlope
        {
            get => _heightOfPierCapSlope;
            set
            {
                _heightOfPierCapSlope = value;
                OnPropertyChanged();
            }
        }
        
        // 抗震挡块参数
        private double _lengthOfShearKey;
        public double LengthOfShearKey
        {
            get => _lengthOfShearKey;
            set
            {
                _lengthOfShearKey = value;
                OnPropertyChanged();
            }
        }
        private double _widthOfShearKey;
        public double WidthOfShearKey
        {
            get => _widthOfShearKey;
            set
            {
                _widthOfShearKey = value;
                OnPropertyChanged();
            }
        }
        private double _heightOfShearKey;
        public double HeightOfShearKey
        {
            get => _heightOfShearKey;
            set
            {
                _heightOfShearKey = value;
                OnPropertyChanged();
            }
        }
        private double _gapOfShearKey;
        public double GapOfShearKey
        {
            get => _gapOfShearKey;
            set
            {
                _gapOfShearKey = value;
                OnPropertyChanged();
            }
        }
        
        // 支座参数
        private int _rowOfBearing;
        public int RowOfBearing
        {
            get => _rowOfBearing;
            set
            {
                _rowOfBearing = value;
                OnPropertyChanged();
            }
        }
        
        
        private int _columnOfBearing;
        public int ColumnOfBearing
        {
            get => _columnOfBearing;
            set
            {
                _columnOfBearing = value;
                OnPropertyChanged();
            }
        }
        
        private double _rowSpaceOfBearing;
        public double RowSpaceOfBearing
        {
            get => _rowSpaceOfBearing;
            set
            {
                _rowSpaceOfBearing = value;
                OnPropertyChanged();
            }
        }
        
        private double _columnSpaceOfBearing;
        public double ColumnSpaceOfBearing
        {
            get => _columnSpaceOfBearing;
            set
            {
                _columnSpaceOfBearing = value;
                OnPropertyChanged();
            }
        }

        private double _lengthOfBearingPedestal;
        public double LengthOfBearingPedestal
        {
            get => _lengthOfBearingPedestal;
            set
            {
                _lengthOfBearingPedestal = value;
                OnPropertyChanged();
            }
        }

        private double _heightOfBearingPedestal;
        public double HeightOfBearingPedestal
        {
            get => _heightOfBearingPedestal;
            set
            {
                _heightOfBearingPedestal = value;
                OnPropertyChanged();
            }
        }
        
        private double _diameterOfRubberGasket;
        public double DiameterOfRubberGasket
        {
            get => _diameterOfRubberGasket;
            set
            {
                _diameterOfRubberGasket = value;
                OnPropertyChanged();
            }
        }

        private double _heightOfRubberGasket;
        public double HeightOfRubberGasket
        {
            get => _heightOfRubberGasket;
            set
            {
                _heightOfRubberGasket = value;
                OnPropertyChanged();
            }
        }
        
        // 墩柱参数
        private int _numberOfPierColumn;
        public int NumberOfPierColumn
        {
            get => _numberOfPierColumn;
            set
            {
                _numberOfPierColumn = value;
                OnPropertyChanged();
            }
        }
        
        private double _spaceOfPierColumn;
        public double SpaceOfPierColumn
        {
            get => _spaceOfPierColumn;
            set
            {
                _spaceOfPierColumn = value;
                OnPropertyChanged();
            }
        }
        
        private double _heightOfPierColumn;
        public double HeightOfPierColumn
        {
            get => _heightOfPierColumn;
            set
            {
                _heightOfPierColumn = value;
                OnPropertyChanged();
            }
        }

        private double _diameterOfPierColumn;
        public double DiameterOfPierColumn
        {
            get => _diameterOfPierColumn;
            set
            {
                _diameterOfPierColumn = value;
                OnPropertyChanged();
            }
        }
        
        // 系梁参数
        private int _numberOfTieBeam;
        public int NumberOfTieBeam
        {
            get => _numberOfTieBeam;
            set
            {
                _numberOfTieBeam = value;
                OnPropertyChanged();
            }
        }
        
        private double _spaceOfTieBeam;
        public double SpaceOfTieBeam
        {
            get => _spaceOfTieBeam;
            set
            {
                _spaceOfTieBeam = value;
                OnPropertyChanged();
            }
        }
        
        private double _widthOfTieBeam;
        public double WidthOfTieBeam
        {
            get => _widthOfTieBeam;
            set
            {
                _widthOfTieBeam = value;
                OnPropertyChanged();
            }
        }
        
        private double _heightOfTieBeam;
        public double HeightOfTieBeam
        {
            get => _heightOfTieBeam;
            set
            {
                _heightOfTieBeam = value;
                OnPropertyChanged();
            }
        }
        
        // 承台参数
        private double _lengthOfFoundation;
        public double LengthOfFoundation
        {
            get => _lengthOfFoundation;
            set
            {
                _lengthOfFoundation = value;
                OnPropertyChanged();
            }
        }

        private double _widthOfFoundation;
        public double WidthOfFoundation
        {
            get => _widthOfFoundation;
            set
            {
                _widthOfFoundation = value;
                OnPropertyChanged();
            }
        }

        private double _heightOfFoundation;
        public double HeightOfFoundation
        {
            get => _heightOfFoundation;
            set
            {
                _heightOfFoundation = value;
                OnPropertyChanged();
            }
        }
        
        // 桩参数
        private int _rowOfPile;
        public int RowOfPile
        {
            get => _rowOfPile;
            set
            {
                _rowOfPile = value;
                OnPropertyChanged();
            }
        }
        
        private int _columnOfPile;
        public int ColumnOfPile
        {
            get => _columnOfPile;
            set
            {
                _columnOfPile = value;
                OnPropertyChanged();
            }
        }

        private double _rowSpaceOfPile;
        
        public double RowSpaceOfPile
        {
            get => _rowSpaceOfPile;
            set
            {
                _rowSpaceOfPile = value;
                OnPropertyChanged();
            }
        }
        
        private double _columnSpaceOfPile;
        public double ColumnSpaceOfPile
        {
            get => _columnSpaceOfPile;
            set
            {
                _columnSpaceOfPile = value;
                OnPropertyChanged();
            }
        }
        
        private double _diameterOfPile;
        public double DiameterOfPile
        {
            get => _diameterOfPile;
            set
            {
                _diameterOfPile = value;
                OnPropertyChanged();
            }
        }

        private double _heightOfPile;
        public double HeightOfPile
        {
            get => _heightOfPile;
            set
            {
                _heightOfPile = value;
                OnPropertyChanged();
            }
        }
        public ColumnPierModel()
        {
            Type = BridgeComponentType.ColumnPier;
            Name = $"柱式墩_{DateTime.Now:mmss}";
            Mileage = 0;
            BlockTableRecordId = ObjectId.Null;
            BlockTableRecordName = "";
            
            LengthOfPierCap = 1000;
            WidthOfPierCap = 180;
            HeightOfPierCap = 150;
            LengthOfPierCapSlope = 150;
            HeightOfPierCapSlope = 100;
            
            LengthOfShearKey = 50;
            WidthOfShearKey = 180;
            HeightOfShearKey = 50;
            GapOfShearKey = 65;
            
            
            RowOfBearing = 2;
            RowSpaceOfBearing = 100;
            ColumnOfBearing = 4;
            ColumnSpaceOfBearing = 240;
            LengthOfBearingPedestal = 50;
            HeightOfBearingPedestal = 10;
            DiameterOfRubberGasket = 40;
            HeightOfRubberGasket = 10;
            
            NumberOfPierColumn = 2;
            SpaceOfPierColumn = 430;
            HeightOfPierColumn = 1000;
            DiameterOfPierColumn = 120;
            
            NumberOfTieBeam = 2;
            SpaceOfTieBeam = 300;
            WidthOfTieBeam = 60;
            HeightOfTieBeam = 120;
            
            LengthOfFoundation = 800;
            WidthOfFoundation = 400;
            HeightOfFoundation = 120;
            
            RowOfPile = 2;
            ColumnOfPile = 4;
            RowSpaceOfPile = 200;
            ColumnSpaceOfPile = 200;
            DiameterOfPile = 100;
            HeightOfPile = 1500;
        }
    }
}

