using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace HrtzImageViewer.Helpers
{
    /// <summary>
    /// Provides methods for checking whether a file can likely be opened as a BitmapImage, based upon its file extension
    /// </summary>
    public class BitmapImageCheck : IDisposable
    {
        #region class variables

        readonly RegistryKey _baseKey;
        private const string WicDecoderCategory = "{7ED96837-96F0-4812-B211-F13C24117ED3}";

        #endregion

        #region constructors

        public BitmapImageCheck()
        {
            string baseKeyPath;

            if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
                baseKeyPath = "Wow6432Node\\CLSID";
            else
                baseKeyPath = "CLSID";

            _baseKey = Registry.ClassesRoot.OpenSubKey(baseKeyPath, false);

            RecalculateExtensions();
        }

        #endregion

        #region properties

        /// <summary>
        /// File extensions that are supported by decoders found elsewhere on the system
        /// </summary>
        public string[] CustomSupportedExtensions { get; private set; }

        /// <summary>
        /// File extensions that are supported natively by .NET
        /// </summary>
        public string[] NativeSupportedExtensions { get; private set; }

        /// <summary>
        /// File extensions that are supported both natively by NET, and by decoders found elsewhere on the system
        /// </summary>
        public string[] AllSupportedExtensions { get; private set; }

        #endregion

        #region public methods

        /// <summary>
        /// Check whether a file is likely to be supported by BitmapImage based upon its extension
        /// </summary>
        /// <param name="extension">File extension (with or without leading full stop), file name or file path</param>
        /// <returns>True if extension appears to contain a supported file extension, false if no suitable extension was found</returns>
        public bool IsExtensionSupported(string extension)
        {
            //prepare extension, should a full path be given
            if (extension.Contains("."))
                extension = extension.Substring(extension.LastIndexOf('.') + 1);

            extension = extension.ToUpper();
            extension = extension.Insert(0, ".");

            return AllSupportedExtensions.Contains(extension);
        }

        #endregion

        #region private methods

        /// <summary>
        /// Re-calculate which extensions are available on this system. It's unlikely this ever needs to be called outside of the constructor.
        /// </summary>
        private void RecalculateExtensions()
        {
            CustomSupportedExtensions = GetSupportedExtensions().ToArray();
            NativeSupportedExtensions = new[] { ".BMP", ".GIF", ".ICO", ".JPEG", ".PNG", ".TIFF", ".DDS", ".JPG", ".JXR", ".HDP", ".WDP" };

            var cse = CustomSupportedExtensions;
            var nse = NativeSupportedExtensions;
            var ase = new string[cse.Length + nse.Length];
            Array.Copy(nse, ase, nse.Length);
            Array.Copy(cse, 0, ase, nse.Length, cse.Length);
            AllSupportedExtensions = ase;
        }

        /// <summary>
        /// Represents information about a WIC decoder
        /// </summary>
        private struct DecoderInfo
        {
            public string FriendlyName;
            public string FileExtensions;
        }

        /// <summary>
        /// Gets a list of additionally registered WIC decoders
        /// </summary>
        /// <returns></returns>
        private IEnumerable<DecoderInfo> GetAdditionalDecoders()
        {
            return GetCodecKeys().Select(codecKey => new DecoderInfo
            {
                FriendlyName = Convert.ToString(codecKey.GetValue("FriendlyName", string.Empty)), FileExtensions = Convert.ToString(codecKey.GetValue("FileExtensions", string.Empty))
            }).ToList();
        }

        private List<string> GetSupportedExtensions()
        {
            var decoders = GetAdditionalDecoders();

            return decoders.SelectMany(decoder => decoder.FileExtensions.Split(',')).ToList();
        }

        private IEnumerable<RegistryKey> GetCodecKeys()
        {
            var result = new List<RegistryKey>();

            var categoryKey = _baseKey?.OpenSubKey(WicDecoderCategory + "\\instance", false);
            if (categoryKey == null) return result;

            result.AddRange(GetCodecGuids().Select(codecGuid => _baseKey.OpenSubKey(codecGuid)).Where(codecKey => codecKey != null));

            return result;
        }

        private IEnumerable<string> GetCodecGuids()
        {
            var categoryKey = _baseKey?.OpenSubKey(WicDecoderCategory + "\\instance", false);

            // Read the guids of the registered decoders
            return categoryKey?.GetSubKeyNames();
        }

        #endregion


        #region overrides and whatnot

        public override string ToString()
        {
            var rtnstring = string.Empty;

            rtnstring += "\nNative support for the following extensions is available: ";
            rtnstring = NativeSupportedExtensions.Aggregate(rtnstring, (current, item) => current + (item + ","));
            if (NativeSupportedExtensions.Any()) rtnstring = rtnstring.Remove(rtnstring.Length - 1);

            var decoders = GetAdditionalDecoders();
            var decoderInfos = decoders as DecoderInfo[] ?? decoders.ToArray();
            if (decoderInfos.Count() == 0) rtnstring += "\n\nNo custom decoders found.";
            else
            {
                rtnstring += "\n\nThese custom decoders were also found:";
                rtnstring = decoderInfos.Aggregate(rtnstring, (current, decoder) => current + ("\n" + decoder.FriendlyName + ", supporting extensions " + decoder.FileExtensions));
            }

            return rtnstring;
        }

        public void Dispose()
        {
            _baseKey.Dispose();
        }

        #endregion
    }
}
