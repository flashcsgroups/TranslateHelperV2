using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PortableCore.BL.Contracts;
using Android.Graphics;
using PortableCore.BL.Managers;

//По мотивам:
//https://forums.xamarin.com/discussion/6029/how-to-create-ui-elements-dynamically

namespace TranslateHelper.Droid
{
    public class DynamicResultViewLayout : LinearLayout
    {
        private Activity context;
        private List<TranslateResultDefinition> definitions;

        public DynamicResultViewLayout(Activity context, List<TranslateResultDefinition> definitions) : base(context)
        {
            this.definitions = definitions;
            this.context = context;
            this.Initialize();
        }

        private void Initialize()
        {
            this.Orientation = Orientation.Vertical;
            foreach(var def in definitions)
            {
                LinearLayout llHeader = new LinearLayout(context);
                llHeader.Orientation = Orientation.Horizontal;
                this.AddView(llHeader);
                LinearLayout llData = new LinearLayout(context);
                llData.Orientation = Orientation.Horizontal;
                this.AddView(llData);

                TextView OriginalTextTextView = new TextView(context);
                OriginalTextTextView.Text = def.OriginalText;
                OriginalTextTextView.SetTextAppearance(context, Resource.Style.VariantHeaderTextView);
                llHeader.AddView(OriginalTextTextView);

                TextView OriginalTextTranscriptionTextView = new TextView(context);
                OriginalTextTranscriptionTextView.Text = string.Format(" [{0}]", def.Transcription);
                OriginalTextTranscriptionTextView.SetTextAppearance(context, Resource.Style.VariantHeaderTextView);
                llHeader.AddView(OriginalTextTranscriptionTextView);

                TextView PosTextView = new TextView(context);
                PosTextView.Text = string.Format(", {0}", DefinitionTypesManager.GetRusNameForEnum(def.Pos));
                PosTextView.SetTextAppearance(context, Resource.Style.VariantHeaderTextView);
                llHeader.AddView(PosTextView);

                ListView lvVariants = new ListView(context);
                lvVariants.FastScrollEnabled = true;

                llData.AddView(lvVariants);
                lvVariants.Adapter = new TranslateResultViewVariantAdapter(context, def.TranslateVariants);
            }

        }
    }
}