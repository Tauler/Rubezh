﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using FiresecClient;

namespace DevicesModule.Events
{
    public class ZoneSelectedEvent : CompositePresentationEvent<string>
    {
    }
}