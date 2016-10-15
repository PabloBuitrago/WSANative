﻿using System.Collections.Generic;
using CI.WSANative.Advertising;
using CI.WSANative.Device;
using CI.WSANative.Dialogs;
using CI.WSANative.Facebook;
using CI.WSANative.FilePickers;
using CI.WSANative.FileStorage;
using CI.WSANative.Geolocation;
using CI.WSANative.IAPStore;
using CI.WSANative.Mapping;
using CI.WSANative.Notification;
using CI.WSANative.Security;
using CI.WSANative.Serialisers;
using CI.WSANative.Web;
using UnityEngine;

public class ExampleSceneManagerController : MonoBehaviour
{
    public void Start()
    {
        // Uncomment these lines when testing in app purchases 
        // ReloadSimulator will throw an exception if it is not correctly configured - see website for details

        //WSANativeStore.EnableTestMode();

        //WSANativeStore.ReloadSimulator();
    }

    public void CreateDialog()
    {
        WSANativeDialog.ShowDialogWithOptions("This is a title", "This is a message", new List<WSADialogCommand>() { new WSADialogCommand("Yes"), new WSADialogCommand("No"), new WSADialogCommand("Cancel") }, 0, 2, (WSADialogResult result) =>
        {
            if (result.ButtonPressed == "Yes")
            {
                WSANativeDialog.ShowDialog("Yes Pressed", "Yes was pressed!");
            }
            else if (result.ButtonPressed == "No")
            {
                WSANativeDialog.ShowDialog("No Pressed", "No was pressed!");
            }
            else if (result.ButtonPressed == "Cancel")
            {
                WSANativeDialog.ShowDialog("Cancel Pressed", "Cancel was pressed!");
            }
        });
    }

    public void CreatePopupMenu()
    {
        WSANativePopupMenu.ShowPopupMenu(Screen.width / 2, Screen.height / 2, new List<WSADialogCommand>() { new WSADialogCommand("Option 1"), new WSADialogCommand("Option 2"), new WSADialogCommand("Option 3"), new WSADialogCommand("Option 4"), new WSADialogCommand("Option 5"), new WSADialogCommand("Option 6") }, WSAPopupMenuPlacement.Above, (WSADialogResult result) =>
        {
            if (result.ButtonPressed == "Yes")
            {
                WSANativeDialog.ShowDialog("Yes Pressed", "Yes was pressed!");
            }
            else if (result.ButtonPressed == "No")
            {
                WSANativeDialog.ShowDialog("No Pressed", "No was pressed!");
            }
            else if (result.ButtonPressed == "Cancel")
            {
                WSANativeDialog.ShowDialog("Cancel Pressed", "Cancel was pressed!");
            }
        });
    }

    public void CreateToastNotification()
    {
        WSANativeNotification.ShowToastNotification("This is a title", "This is a description, This is a description, This is a description");
    }

    public void SaveFile()
    {
        string result = WSANativeSerialisation.SerialiseToXML(new Test());
        WSANativeStorage.SaveFile("Test.txt", result);
    }

    public void LoadFile()
    {
        if (WSANativeStorage.DoesFileExist("Test.txt"))
        {
            string result = WSANativeStorage.LoadFile("Test.txt");
            WSANativeSerialisation.DeserialiseXML<Test>(result);
        }
    }

    public void DeleteFile()
    {
        if (WSANativeStorage.DoesFileExist("Test.txt"))
        {
            WSANativeStorage.DeleteFile("Test.txt");
        }
    }

    public void PurchaseProduct()
    {
        WSANativeStore.GetProductListings((List<WSAProduct> products) =>
        {
            if (products != null && products.Count > 0)
            {
                WSANativeStore.PurchaseProduct(products[0].Id, (WSAPurchaseResult result) =>
                {
                    if (result.Status == WSAPurchaseResultStatus.Succeeded)
                    {
                        WSANativeDialog.ShowDialog("Purchased", "YAY");
                    }
                    else
                    {
                        WSANativeDialog.ShowDialog("Not Purchased", "NAY");
                    }
                });
            }
        });
    }

    public void PurchaseApp()
    {
        WSANativeStore.PurchaseApp((string response) =>
        {
            if (!string.IsNullOrEmpty(response))
            {
                WSANativeDialog.ShowDialog("Purchased", response);
            }
            else
            {
                WSANativeDialog.ShowDialog("Not Purchased", "NAY");
            }
        });
    }

    public void ShowFileOpenPicker()
    {
        WSANativeFilePicker.PickSingleFile("Select", WSAPickerViewMode.Thumbnail, WSAPickerLocationId.PicturesLibrary, new[] { ".png", ".jpg" }, (result) =>
        {
            if (result != null)
            {
                byte[] fileBytes = result.ReadBytes();
                string fileString = result.ReadText();
            }
        });
    }

    public void ShowFileSavePicker()
    {
        WSANativeFilePicker.PickSaveFile("Save", ".jpg", "Test Image", WSAPickerLocationId.DocumentsLibrary, new List<KeyValuePair<string, IList<string>>>() { new KeyValuePair<string, IList<string>>("Image Files", new List<string>() { ".png", ".jpg" }) }, (result) =>
        {
            if (result != null)
            {
                result.WriteBytes(new byte[2]);
                result.WriteText("Hello World");
            }
        });
    }

    public void WebGet()
    {
        WSANativeWeb.GetBytes("http://httpbin.org/encoding/utf8", (success, response) =>
        {
        });
    }

    public void CreateInterstitialAd()
    {
        WSANativeInterstitialAd.InitialiseMicrosoft("d25517cb-12d4-4699-8bdc-52040c712cab", "11389925");
        WSANativeInterstitialAd.AdReady += (adType) =>
        {
            WSANativeInterstitialAd.ShowAd(WSAInterstitialAdType.Microsoft);
        };
        WSANativeInterstitialAd.RequestAd(WSAInterstitialAdType.Microsoft);
    }

    public void CreateBannerAd()
    {
        WSANativeBannerAd.CreatAd("d25517cb-12d4-4699-8bdc-52040c712cab", "10042998", 728, 90, WSAAdVerticalPlacement.Top, WSAAdHorizontalPlacement.Centre);
    }

    public void CreateMediatedAd()
    {
        WSANativeMediatorAd.CreatAd("3f83fe91-d6be-434d-a0ae-7351c5a997f1", "10865270", "2c41122f-82fa-43d9-8513-610b5fb9df45", "198067", 50,
            728, 90, WSAAdVerticalPlacement.Top, WSAAdHorizontalPlacement.Centre);
    }

    public void CreateMap()
    {
        int xPos = (Screen.width / 2) - 350;
        int yPos = (Screen.height / 2) - 350;

        WSANativeMap.CreateMap(string.Empty, 700, 700, new WSAPosition() { X = xPos, Y = yPos }, new WSAGeoPoint() { Latitude = 50, Longitude = 0 }, 6, WSAMapInteractionMode.GestureAndControl);
    }

    public void DestroyMap()
    {
        WSANativeMap.DestroyMap();
    }

    public void AddPOI()
    {
        WSANativeMap.AddMapElement("You are here", new WSAGeoPoint() { Latitude = 52, Longitude = 5 });
    }

    public void ClearMap()
    {
        WSANativeMap.ClearMap();
    }

    public void CenterMap()
    {
        WSANativeMap.CenterMap(new WSAGeoPoint() { Latitude = 52, Longitude = 5 });
    }

    public void GetMyLocation()
    {
        WSANativeGeolocation.GetUsersLocation(10, (response) =>
        {
            if (response.Success)
            {
                WSANativeDialog.ShowDialog("My Location Is", string.Format("Latitude: {0} Longitude: {1}", response.GeoPosition.Latitude, response.GeoPosition.Longitude));
            }
            else
            {
                WSANativeDialog.ShowDialog("Failed to get location", string.Format("Access was {0}", response.AccessStatus.ToString()));
            }
        });
    }

    public void EnableFlashlight()
    {
        WSANativeDevice.EnableFlashlight(new WSANativeColour() { Red = 0, Green = 0, Blue = 255 });
    }

    public void DisableFlashlight()
    {
        WSANativeDevice.DisableFlashlight();
    }

    public void EncryptDecrypt()
    {
        string encrypted = WSANativeSecurity.SymmetricEncrypt("ffffffffffffffffffffffffffffffff", "aaaaaaaaaaaaaaaa", "Tesing123");

        WSANativeSecurity.SymmetricDecrypt("ffffffffffffffffffffffffffffffff", "aaaaaaaaaaaaaaaa", encrypted);
    }

    public void FacebookLogin()
    {
        WSANativeFacebook.Initialise("facebookId", "packageSID");
        WSANativeFacebook.Login(new List<string>() { "public_profile", "email", "user_birthday" }, (success) =>
        {
            if (success)
            {
                WSANativeFacebook.GetUserDetails((response) =>
                {
                    if (response.Success)
                    {
                        WSAFacebookUser user = response.Data;
                    }
                });
            }
        });
    }
}

public class Test
{
    public int x = 10;
    public float y = 20.56f;
    public string s = "Hello World";
}