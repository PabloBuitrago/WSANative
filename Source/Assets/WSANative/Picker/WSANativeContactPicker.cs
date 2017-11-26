﻿using System;
using System.Collections.Generic;

#if NETFX_CORE
using System.Linq;
using Windows.ApplicationModel.Contacts;
#endif

namespace CI.WSANative.Pickers
{
    public class WSANativeContactPicker
    {
        /// <summary>
        /// Launches a picker which allows the user to choose a contact
        /// </summary>
        /// <param name="response">Contains the chosen contact or null if nothing was selected</param>
        public static void PickContact(Action<WSAContact> response)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                ContactPicker contactPicker = new ContactPicker();

                contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Email);
                contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
                contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Address);

                Contact contact = await contactPicker.PickContactAsync();

                UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                {
                    if (response != null)
                    {
                        response(contact != null ? MapContactToWSAContact(contact) : null);
                    }
                }, true);
            }, false);
#endif
        }

        /// <summary>
        /// Launches a picker which allows the user to choose multiple contacts
        /// </summary>
        /// <param name="response">Contains the chosen contacts or null if nothing was selected</param>
        public static void PickContacts(Action<IEnumerable<WSAContact>> response)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                ContactPicker contactPicker = new ContactPicker();

                contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Email);
                contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
                contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Address);

                IList<Contact> contacts = await contactPicker.PickContactsAsync();

                UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                {
                    if (response != null)
                    {
                        response(contacts != null && contacts.Any() ? contacts.Select(x => MapContactToWSAContact(x)) : null);
                    }
                }, true);
            }, false);
#endif
        }

#if NETFX_CORE
        private static WSAContact MapContactToWSAContact(Contact contact)
        {
            return new WSAContact()
            {
                DisplayName = contact.DisplayName,
                FullName = contact.FullName,
                FirstName = contact.FirstName,
                MiddleName = contact.MiddleName,
                LastName = contact.LastName,
                Nickname = contact.Nickname,
                Emails = contact.Emails.Select(x => x.Address).ToList(),
                Phones = contact.Phones.Select(x => x.Number).ToList(),
                OriginalContact = contact
            };
        }
#endif
    }
}