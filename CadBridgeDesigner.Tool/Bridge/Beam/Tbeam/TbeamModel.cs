using System;
using Autodesk.AutoCAD.DatabaseServices;
using CadBridgeDesigner.Tool.Bridge.Base;

namespace CadBridgeDesigner.Tool.Bridge.Beam.Tbeam
{
    public sealed class TbeamModel: BridgeComponentModel
    {
        private double _beamEndGap;
        public double BeamEndGap
        {
            get => _beamEndGap;
            set
            {
                _beamEndGap = value;
                OnPropertyChanged();
            }
        }
        
        private double _spanLength;
        public double SpanLength
        {
            get => _spanLength;
            set
            {
                _spanLength = value;
                OnPropertyChanged();
            }
        }
        
        private double _height;
        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }
        
        private int _numberOfTbeam;
        public int NumberOfTbeam
        {
            get => _numberOfTbeam;
            set
            {
                _numberOfTbeam = value;
                OnPropertyChanged();
            }
        }
        
        private double _widthOfFlange;
        public double WidthOfFlange
        {
            get => _widthOfFlange;
            set
            {
                _widthOfFlange = value;
                OnPropertyChanged();
            }
        }
        
        private double _heightOfFlange;
        public double HeightOfFlange
        {
            get => _heightOfFlange;
            set
            {
                _heightOfFlange = value;
                OnPropertyChanged();
            }
        }
        
        private double _widthOfFlangeHaunch;
        public double WidthOfFlangeHaunch
        {
            get => _widthOfFlangeHaunch;
            set
            {
                _widthOfFlangeHaunch = value;
                OnPropertyChanged();
            }
        }
        
        private double _heightOfFlangeHaunch;
        public double HeightOfFlangeHaunch
        {
            get => _heightOfFlangeHaunch;
            set
            {
                _heightOfFlangeHaunch = value;
                OnPropertyChanged();
            }
        }
        
        private double _widthOfWeb;
        public double WidthOfWeb
        {
            get => _widthOfWeb;
            set
            {
                _widthOfWeb = value;
                OnPropertyChanged();
            }
        }
        
        private double _widthOfButtonBulb;
        public double WidthOfButtonBulb
        {
            get => _widthOfButtonBulb;
            set
            {
                _widthOfButtonBulb = value;
                OnPropertyChanged();
            }
        }
        
        private double _heightOfButtonBulb;
        public double HeightOfButtonBulb
        {
            get => _heightOfButtonBulb;
            set
            {
                _heightOfButtonBulb = value;
                OnPropertyChanged();
            }
        }
        
        private double _heightOfButtonBulbSlope;
        public double HeightOfButtonBulbSlope
        {
            get => _heightOfButtonBulbSlope;
            set
            {
                _heightOfButtonBulbSlope = value;
                OnPropertyChanged();
            }
        }

        private double _distanceOfStartOfCrossSection;
        public double DistanceOfStartOfCrossSection
        {
            get => _distanceOfStartOfCrossSection;
            set
            {
                _distanceOfStartOfCrossSection = value;
                OnPropertyChanged();
            }
        }
        
        private double _distanceOfEndOfCrossSection;
        public double DistanceOfEndOfCrossSection
        {
            get => _distanceOfEndOfCrossSection;
            set
            {
                _distanceOfEndOfCrossSection = value;
                OnPropertyChanged();
            }
        }
        
        private double _spaceOfDiaphragm;
        public double SpaceOfDiaphragm
        {
            get => _spaceOfDiaphragm;
            set
            {
                _spaceOfDiaphragm = value;
                OnPropertyChanged();
            }
        }
        
        private double _thicknessOfDiaphragm;
        public double ThicknessOfDiaphragm
        {
            get => _thicknessOfDiaphragm;
            set
            {
                _thicknessOfDiaphragm = value;
                OnPropertyChanged();
            }
        }
        
        private int _numberOfMiddleDiaphragm;
        public int NumberOfMiddleDiaphragm
        {
            get => _numberOfMiddleDiaphragm;
            set
            {
                _numberOfMiddleDiaphragm = value;
                OnPropertyChanged();
            }
        }
        
        private double _distanceOfEndDiaphragm;
        public double DistanceOfEndDiaphragm
        {
            get => _distanceOfEndDiaphragm;
            set
            {
                _distanceOfEndDiaphragm = value;
                OnPropertyChanged();
            }
        }

        private double _widthOfWetJoint;
        public double WidthOfWetJoint
        {
            get => _widthOfWetJoint;
            set
            {
                _widthOfWetJoint = value;
                OnPropertyChanged();
            }
        }
        
        
        
        
        public TbeamModel()
        {
            Type = BridgeComponentType.Tbeam;
            Name = $"T梁_{DateTime.Now:mmss}";
            Mileage = 0;
            BlockTableRecordId = ObjectId.Null;
            BlockTableRecordName = "";
            BeamEndGap = 15;
            SpanLength = 3000;
            Height = 200;
            NumberOfTbeam = 4;
            DistanceOfStartOfCrossSection = 300;
            DistanceOfEndOfCrossSection = 750;
            
            WidthOfFlange = 240;
            HeightOfFlange = 15;
            WidthOfFlangeHaunch = 40;
            HeightOfFlangeHaunch = 12.5;
            
            WidthOfWeb = 20;
            
            
            WidthOfButtonBulb = 50;
            HeightOfButtonBulb = 30;
            HeightOfButtonBulbSlope = 10;
            
            SpaceOfDiaphragm = 600;
            ThicknessOfDiaphragm = 15;
            NumberOfMiddleDiaphragm = 3;
            DistanceOfEndDiaphragm = 50;
            
            WidthOfWetJoint = 60;
        }

    }
}

