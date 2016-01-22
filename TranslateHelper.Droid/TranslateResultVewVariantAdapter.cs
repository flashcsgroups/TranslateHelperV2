﻿using Android.App;
using Android.Views;
using Android.Widget;
using PortableCore.BL.Contracts;
using PortableCore.BL.Managers;
using System;
using System.Collections.Generic;

namespace TranslateHelper.Droid
{

    public class TranslateResultViewVariantAdapter : BaseAdapter<TranslateResultViewVariantAdapter.DataOfOneLine>
    {
        protected Activity context = null;
        private List<DataOfOneLine> listResultLines = new List<DataOfOneLine>();
        private List<TranslateResultDefinition> definitions;
        private int indexLineInGroup = 0;

        public TranslateResultViewVariantAdapter(Activity context, List<TranslateResultDefinition> definitions)
            : base()
        {
            this.context = context;
            this.definitions = definitions;
            this.listResultLines = extractDefinitionsToLines(definitions);
        }

        private List<DataOfOneLine> extractDefinitionsToLines(List<TranslateResultDefinition> definitions)
        {
            List<DataOfOneLine> resultLine = new List<DataOfOneLine>();
            foreach(var itemDef in definitions)
            {
                var groupItem = new DataOfOneLine();
                groupItem.IsGroup = true;
                groupItem.Definition = itemDef;
                resultLine.Add(groupItem);
                foreach (var itemVariant in itemDef.TranslateVariants)
                {
                    var item = new DataOfOneLine();
                    item.IsGroup = false;
                    item.Definition = itemDef;
                    item.TranslateVariant = itemVariant;
                    resultLine.Add(item);
                }
            }
            return resultLine;
        }

        public override DataOfOneLine this[int position]
        {
            get { return listResultLines[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return listResultLines.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = listResultLines[position];
            return item.IsGroup ? getHeaderView(item, position, convertView, parent) : getVariantView(item, position, convertView, parent);
        }

        private LinearLayout getVariantView(DataOfOneLine item, int position, View convertView, ViewGroup parent)
        {
            //ToDo:придумать, как не создавать View каждый раз, учитывая что их 2 разных - для хедера и детальной записи
            //var viewVariant = (convertView ?? this.context.LayoutInflater.Inflate(Resource.Layout.TranslateResultVariant, parent, false)) as LinearLayout;
            var viewVariant = this.context.LayoutInflater.Inflate(Resource.Layout.TranslateResultVariant, parent, false) as LinearLayout;
            var indexTextView = viewVariant.FindViewById<TextView>(Resource.Id.IndexTextView);
            string indexString = string.Empty;
            if(item.Definition.TranslateVariants.Count > 1)
                indexString = (++indexLineInGroup).ToString();

            indexTextView.SetText(indexString, TextView.BufferType.Normal);
            var translatedTextTextView = viewVariant.FindViewById<TextView>(Resource.Id.TranslatedTextTextView);
            translatedTextTextView.SetText(item.TranslateVariant.Text, TextView.BufferType.Normal);
            return viewVariant;
        }

        private LinearLayout getHeaderView(DataOfOneLine item, int position, View convertView, ViewGroup parent)
        {
            //var viewHeader = (convertView ?? this.context.LayoutInflater.Inflate(Resource.Layout.TranslateResultHeader, parent, false)) as LinearLayout;
            var viewHeader = this.context.LayoutInflater.Inflate(Resource.Layout.TranslateResultHeader, parent, false) as LinearLayout;
            var originalTextView = viewHeader.FindViewById<TextView>(Resource.Id.OriginalTextTextView);
            originalTextView.SetText(item.Definition.OriginalText, TextView.BufferType.Normal);
            var transcriptionTextView = viewHeader.FindViewById<TextView>(Resource.Id.OriginalTextTranscriptionTextView);
            transcriptionTextView.SetText(string.Format("[{0}]", item.Definition.Transcription), TextView.BufferType.Normal);
            var posTextView = viewHeader.FindViewById<TextView>(Resource.Id.PosTextView);
            posTextView.SetText(DefinitionTypesManager.GetRusNameForEnum(item.Definition.Pos), TextView.BufferType.Normal);
            indexLineInGroup = 0;
            return viewHeader;
        }

        public class DataOfOneLine
        {
            internal TranslateResultDefinition Definition;
            internal bool IsGroup;
            internal ResultLineData TranslateVariant;
        }
    }
}