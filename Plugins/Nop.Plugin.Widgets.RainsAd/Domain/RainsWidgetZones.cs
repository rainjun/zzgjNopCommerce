using Nop.Core;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.RainsAd.Domain
{
    /// <summary>
    /// �Զ���ĹҼ�������
    /// </summary>
    public partial class RainsWidgetZones : BaseEntity
    {
        public string Name { get; set; }
        public string WidgetZone { get; set; }
        public string WidgetName { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime UpdateOn { get; set; }

    }
}