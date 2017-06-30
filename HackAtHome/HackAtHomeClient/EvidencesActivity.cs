using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Entities;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName")]
    public class EvidencesActivity : Activity
    {
        EvidencesFragment Evidences;
        string Token;
        string FullName;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Evidences);

            if(Intent != null)
            {
                Token = Intent.GetStringExtra("Token");
                FullName = Intent.GetStringExtra("FullName");
            }
            var ServiceClient = new SAL.ServiceClient();
            
            Evidences = (EvidencesFragment)this.FragmentManager.FindFragmentByTag("Evidences");
            if (Evidences == null)
            {
                Evidences = new EvidencesFragment();
                Evidences.Evidences = await ServiceClient.GetEvidencesAsync(Token);
                var FragmentTransaction = this.FragmentManager.BeginTransaction();
                FragmentTransaction.Add(Evidences, "Evidences");
                FragmentTransaction.Commit();
            }            

            var FullNameText = FindViewById<TextView>(Resource.Id.textViewName);
            FullNameText.Text = FullName;
            var ListEvidences = FindViewById<ListView>(Resource.Id.listViewEvidences);
            ListEvidences.Adapter = new CustomAdapters.EvidencesAdapter(
                this, Evidences.Evidences, Resource.Layout.ListItem, Resource.Id.textViewTitle,
                Resource.Id.textViewStatus);
            ListEvidences.ItemClick += ListEvidencesLoad;
        }

        private void ListEvidencesLoad(object sender, AdapterView.ItemClickEventArgs e)
        {
            
            var ActivityIntent = new Intent(this, typeof(EvidenceDetailActivity));
            ActivityIntent.PutExtra("Token", Token);
            ActivityIntent.PutExtra("FullName", FullName);
            ActivityIntent.PutExtra("EvidenceID", Evidences.Evidences[e.Position].EvidenceID);
            ActivityIntent.PutExtra("Title", Evidences.Evidences[e.Position].Title);
            ActivityIntent.PutExtra("Status", Evidences.Evidences[e.Position].Status);
            StartActivity(ActivityIntent);
        }
    }
}