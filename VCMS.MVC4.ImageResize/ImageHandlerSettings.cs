using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace VCMS.MVC4.ImageResize
{
    public class ImageHandlerSettings : ConfigurationSection
    {
        [ConfigurationProperty("settings")]
        public ImageHandlerSettingColletion Settings
        {
            get { return ((ImageHandlerSettingColletion)(base["settings"])); }
        }


    }

    public class ImageHandlerSettingElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }

        }
        [ConfigurationProperty("matchPattern", DefaultValue = "", IsRequired = false)]
        public string MatchPattern
        {
            get { return this["matchPattern"].ToString(); }
            set { this["matchPattern"] = value; }
        }

        [ConfigurationProperty("defaultImageVirtualPath", DefaultValue = "", IsRequired = false)]
        public string DefaultImageVirtualPath
        {
            get { return this["defaultImageVirtualPath"].ToString(); }
            set { this["defaultImageVirtualPath"] = value; }
        }
        [ConfigurationProperty("defaultWidth", DefaultValue = 200, IsRequired = false)]
        public int DefaultWidth
        {
            get { return (int)this["defaultWidth"]; }
            set { this["defaultWidth"] = value; }
        }
        [ConfigurationProperty("watermark", DefaultValue = null, IsRequired = false)]
        public WatermarkSettingElement Watermark
        {
            get
            {
                if (this["watermark"] == null)
                    return null;
                else
                    return (WatermarkSettingElement)this["watermark"];
            }
            set { this["watermark"] = value; }
        }
    }
    public class WatermarkSettingElement : ConfigurationElement
    {
        [ConfigurationProperty("text", DefaultValue = "", IsRequired = false)]
        public string Text
        {
            get { return this["text"].ToString(); }
            set { this["text"] = value; }
        }
        [ConfigurationProperty("position", DefaultValue = WaterMarkPosition.HorizontalMiddle, IsRequired = false)]
        public WaterMarkPosition Position
        {
            get { return (WaterMarkPosition)this["position"]; }
            set { this["position"] = value; }
        }

        [ConfigurationProperty("borderColor", DefaultValue = "Transparent", IsRequired = false)]
        public string Color
        {
            get { return (string)this["borderColor"]; }
            set { this["borderColor"] = value; }
        }
        [ConfigurationProperty("fillColor", DefaultValue = "Transparent", IsRequired = false)]
        public string FillColor
        {
            get { return (string)this["fillColor"]; }
            set { this["fillColor"] = value; }
        }
        [ConfigurationProperty("opacity", DefaultValue = "50", IsRequired = false)]
        public int Opacity
        {
            get { return (int)this["opacity"]; }
            set { this["opacity"] = value; }
        }
        [ConfigurationProperty("fontName", DefaultValue = "Arial", IsRequired = false)]
        public string FontName
        {
            get { return (string)this["fontName"]; }
            set { this["fontName"] = value; }
        }
    }
    [ConfigurationCollection(typeof(ImageHandlerSettingElement), CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
    public class ImageHandlerSettingColletion : ConfigurationElementCollection
    {
        internal const string ItemPropertyName = "item";

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }

        protected override string ElementName
        {
            get { return ItemPropertyName; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ImageHandlerSettingElement();
        }


        public ImageHandlerSettingElement this[int idx]
        {
            get
            {
                return (ImageHandlerSettingElement)BaseGet(idx);
            }
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ImageHandlerSettingElement)(element)).Name;
        }
    }


}
