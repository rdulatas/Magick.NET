﻿// Copyright 2013-2021 Dirk Lemstra <https://github.com/dlemstra/Magick.NET/>
//
// Licensed under the ImageMagick License (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
//
//   https://www.imagemagick.org/script/license.php
//
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. See the License for the specific language governing permissions
// and limitations under the License.

using System.IO;

namespace ImageMagick.Configuration
{
    internal sealed class ConfigurationFile : IConfigurationFile
    {
        public ConfigurationFile(string fileName)
        {
            FileName = fileName;
            Data = LoadData();
        }

        public string FileName { get; }

        public string Data { get; set; }

        private string LoadData()
        {
            using (Stream stream = TypeHelper.GetManifestResourceStream(typeof(ConfigurationFile), "ImageMagick.Resources.Xml", FileName))
            {
                using (var reader = new StreamReader(stream))
                {
                    var data = reader.ReadToEnd();

                    data = UpdateDelegatesXml(data);

                    return data;
                }
            }
        }

        private string UpdateDelegatesXml(string data)
        {
            if (OperatingSystem.IsWindows || FileName != "delegates.xml")
                return data;

            data = data.Replace("@PSDelegate@", "gs");
            data = data.Replace("ffmpeg.exe", "ffmpeg");

            return data;
        }
    }
}