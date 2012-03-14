﻿using System;
using System.Collections.Generic;
using System.Linq;
using FiresecAPI.Models;
using Infrastructure;
using Infrastructure.Common;

namespace SoundsModule.ViewModels
{
    public class SoundViewModel : BaseViewModel
    {
        public const string DefaultName = "<нет>";

        public SoundViewModel(Sound sound)
        {
            Sound = sound;
        }

        public Sound Sound { get; set; }

        public StateType StateType
        {
            get { return Sound.StateType; }
        }

        public string SoundName
        {
            get { return Sound.SoundName; }
            set
            {
                Sound.SoundName = value;
                OnPropertyChanged("SoundName");
                ServiceFactory.SaveService.SoundsChanged = true;
            }
        }

        public BeeperType BeeperType
        {
            get { return Sound.BeeperType; }
            set
            {
                Sound.BeeperType = value;
                OnPropertyChanged("BeeperType");
                ServiceFactory.SaveService.SoundsChanged = true;
            }
        }

        public bool IsContinious
        {
            get { return Sound.IsContinious; }
            set
            {
                Sound.IsContinious = value;
                OnPropertyChanged("IsContinious");
                ServiceFactory.SaveService.SoundsChanged = true;
            }
        }

        public List<string> AvailableSounds
        {
            get
            {
                var listSounds = new List<string>();
                listSounds.Add(string.Empty);
                listSounds.AddRange(FiresecClient.FileHelper.SoundsList);
                return listSounds;
            }
        }

        public List<BeeperType> AvailableSpeakers
        {
            get
            {
                return new List<BeeperType>(Enum.GetValues(typeof(BeeperType)).OfType<BeeperType>());
            }
        }
    }
}