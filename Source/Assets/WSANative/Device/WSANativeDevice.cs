﻿////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE && UNITY_WSA_10_0
using System;
using System.Linq;
using Windows.Devices.Enumeration;
using Windows.Devices.Lights;
#endif

namespace CI.WSANative.Device
{
    public static class WSANativeDevice
    {
#if NETFX_CORE && UNITY_WSA_10_0
        private static Lamp _lamp;
#endif

        /// <summary>
        /// Turns on the flashlight if the device supports it and optionally allows setting of the colour
        /// </summary>
        /// <param name="colour">Set the colour of the flashlight - does nothing if the device doesn't support it</param>
        public static void EnableFlashlight(WSANativeColour colour = null)
        {
#if NETFX_CORE && UNITY_WSA_10_0
            EnableFlashlightAsync(colour);
#endif
        }

#if NETFX_CORE && UNITY_WSA_10_0
        private static async void EnableFlashlightAsync(WSANativeColour colour = null)
        {
            string selectorString = Lamp.GetDeviceSelector();

            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selectorString);

            DeviceInformation deviceInfo =
                devices.FirstOrDefault(di => di.EnclosureLocation != null && 
                    di.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);

            if(deviceInfo != null)
            {
                _lamp = await Lamp.FromIdAsync(deviceInfo.Id);

                if (_lamp.IsColorSettable && colour != null)
                {
                    _lamp.Color = Windows.UI.Color.FromArgb(255, colour.Red, colour.Green, colour.Blue);
                }

                _lamp.IsEnabled = true;
            }
        }
#endif

        /// <summary>
        /// Turns of the flashlight if it is on
        /// </summary>
        public static void DisableFlashlight()
        {
#if NETFX_CORE && UNITY_WSA_10_0
            if(_lamp != null)
            {
                _lamp.IsEnabled = false;
                _lamp.Dispose();
                _lamp = null;
            }
#endif
        }
    }
}