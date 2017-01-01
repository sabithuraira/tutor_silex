// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace silexiOS
{
    [Register ("FormController")]
    partial class FormController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtPrice { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (txtName != null) {
                txtName.Dispose ();
                txtName = null;
            }

            if (txtPrice != null) {
                txtPrice.Dispose ();
                txtPrice = null;
            }
        }
    }
}