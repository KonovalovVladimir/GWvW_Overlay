﻿using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using GWvW_Overlay.Annotations;
using MumbleLink_CSharp_GW2;

namespace GWvW_Overlay.DataModel
{
    public class Positions : INotifyPropertyChanged
    {

        private double _canvasHeight;
        private double _canvasWidth;
        private readonly Timer _refreshData = new Timer(200.0)
        {
            AutoReset = true,
        };
        public GW2Link.Coordinates Player
        {
            get { return Transform(MainWindow.DataLink.GetCoordinates()); }
        }

        public double CanvasHeight
        {
            get { return _canvasHeight; }
            set
            {
                _canvasHeight = value;
                OnPropertyChanged();
            }
        }

        public double CanvasWidth
        {
            get { return _canvasWidth; }
            set
            {
                _canvasWidth = value;
                OnPropertyChanged();
            }
        }


        public Positions()
        {
            _refreshData.Enabled = true;
            _refreshData.Elapsed += (sender, args) =>
            {
                OnPropertyChanged("Player");
            };
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName != "Player")
                {
                    OnPropertyChanged("Player");
                }
            };
        }


        private GW2Link.Coordinates Transform(GW2Link.Coordinates nativeCoordinates)
        {
            var mapId = nativeCoordinates.MapId;

            var mapSize = MapInfo.GetMapInfo(mapId).MapRect;

            var mapSizeX = Math.Abs(mapSize[0][0]) + Math.Abs(mapSize[1][0]);
            var mapSizeY = Math.Abs(mapSize[0][1]) + Math.Abs(mapSize[1][1]);

            nativeCoordinates.X = Math.Abs(CanvasWidth * ((nativeCoordinates.X + mapSizeX / 2.0) / mapSizeX)) - 25.0 / 2.0;
            nativeCoordinates.Y = Math.Abs(CanvasHeight * ((nativeCoordinates.Y - mapSizeY / 2.0) / mapSizeY)) - 29.0 / 2.0;

            return nativeCoordinates;
        }



        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}