using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Entities;
using Android.Webkit;
using Android.Graphics;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName")]
    public class EvidenceDetailActivity : Activity
    {
        string Token;
        string FullName;
        int EvidenceId;
        string TitleEvidence;
        string Status;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EvidenceDetail);

            Token = Intent.GetStringExtra("Token");
            FullName = Intent.GetStringExtra("FullName");
            EvidenceId = Intent.GetIntExtra("EvidenceID", 0);
            TitleEvidence = Intent.GetStringExtra("Title");
            Status = Intent.GetStringExtra("Status");

            FindViewById<TextView>(Resource.Id.textViewName).Text = FullName;
            FindViewById<TextView>(Resource.Id.textViewTitle).Text = TitleEvidence;
            FindViewById<TextView>(Resource.Id.textViewStatus).Text = Status;

            var ServiceClient = new SAL.ServiceClient();
            EvidenceDetail EvidenceDetailByID = await ServiceClient.GetEvidenceByIDAsync(Token, EvidenceId);

            var WebViewDescription = FindViewById<WebView>(Resource.Id.webViewDescription);
            WebViewDescription.LoadDataWithBaseURL(
                null, $"<html><head><style type='text/css'>body{{color:#ffffff}}</style></head><body>{EvidenceDetailByID.Description}</body></html>",
                "text/html", "utf-8", null);
            WebViewDescription.SetBackgroundColor(Color.Transparent);

            var ImageEvidence = FindViewById<ImageView>(Resource.Id.imageViewEvidence);
            Koush.UrlImageViewHelper.SetUrlDrawable(ImageEvidence, EvidenceDetailByID.Url);
        }
    }
}