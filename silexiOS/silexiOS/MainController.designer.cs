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
    [Register ("MainController")]
    partial class MainController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView TabelFood { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel txtTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (TabelFood != null) {
                TabelFood.Dispose ();
                TabelFood = null;
            }

            if (txtTitle != null) {
                txtTitle.Dispose ();
                txtTitle = null;
            }
        }
    }
}