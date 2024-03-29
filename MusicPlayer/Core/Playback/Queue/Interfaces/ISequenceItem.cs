﻿#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using System;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Playback.Queue.Interfaces
{
    public interface ISequenceItem
    {
        TimeSpan Progress { get; set; }
        bool WasCancelled { get; set; }
        Task Play(IQueueMediator mediator);
        void Pause();
        void Stop();
        void SetProgress(TimeSpan progress); 
    }
}
