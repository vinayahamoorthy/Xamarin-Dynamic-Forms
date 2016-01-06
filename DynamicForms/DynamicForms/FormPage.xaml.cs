using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Reflection;
using System.IO;
using Newtonsoft.Json.Linq;

namespace DynamicForms
{
	public partial class FormPage : ContentPage
	{
		public JArray fieldsArray;
		public IDictionary<string,object> bindingModel;
		public FormPage ()
		{
			bindingModel = new Dictionary<string, object> ();

			InitializeComponent ();

			if(fieldsArray == null)
			{
				var assembly = typeof(FormPage).GetTypeInfo ().Assembly;
				Stream stream = assembly.GetManifestResourceStream ("DynamicForms.form1.json");

				string jsonString = "";
				using (var reader = new System.IO.StreamReader (stream)) {
					jsonString = reader.ReadToEnd ();
				}

				fieldsArray = JArray.Parse (jsonString);

				foreach (var arrval in fieldsArray)
				{
					if((string)arrval["ControlType"] == "1")
					{
						var entry = new Entry {
							Placeholder = (string)arrval["DefaultCaption"],
							HorizontalOptions = LayoutOptions.Center,
							WidthRequest=250
						};
						entry.SetBinding(Entry.TextProperty, string.Format("[{0}]", (string)arrval["BindProperty"]));
						bindingModel.Add ((string)arrval ["BindProperty"], arrval ["DefaultVal"]);
						this.FormContainer.Children.Add (entry);
					}
					else if((string)arrval["ControlType"] == "2")
					{
						var button = new Button {
							Text = (string)arrval["DefaultCaption"],
							HorizontalOptions = LayoutOptions.Center
						};
						button.Clicked += OnButtonClicked;
						this.FormContainer.Children.Add (button);
					}
				}
			}

			var bindingcontext = new GenericViewModel
			{
				FormData = bindingModel
			};
			this.BindingContext = bindingcontext.FormData;
		}
		private void OnButtonClicked (object sender, EventArgs e)
		{
			var context = (sender as Button).BindingContext;

			if (context.GetType() ==typeof(Dictionary<string,object>)) {
				var dict = (Dictionary<string,object>)context;
				foreach (var dictitem in dict) {
					Console.WriteLine(string.Format("{0} - {1}",dictitem.Key,dictitem.Value));
				}
			}
		}
	}
}

