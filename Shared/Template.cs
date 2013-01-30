using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using Wizard;

namespace NoxShared
{
	public class Template
	{
		private Assembly scriptAssembly = null;
		private BaseClass scriptClass = new BaseClass();
		private Info mInfo;
		private Map map = null;
		private string baseDir = null;

		public Template(string name, Map mMap)
		{
			map = mMap;
			mInfo = new Info(map);
			baseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+Path.DirectorySeparatorChar+"Templates/" + name + Path.DirectorySeparatorChar;
			parseTemplateXml(baseDir + name + ".xml");
			if(Directory.GetFiles(baseDir,"*.cs").Length > 0)
				execScript(baseDir);
		}

		public bool execScript(string dirPath)
		{
			Microsoft.CSharp.CSharpCodeProvider csharp = new Microsoft.CSharp.CSharpCodeProvider();
			// Setup default params for compiler
			CompilerParameters compilerParams = new CompilerParameters();
			compilerParams.GenerateExecutable = false;
			compilerParams.GenerateInMemory = true;
			compilerParams.IncludeDebugInformation = false;
			compilerParams.ReferencedAssemblies.Add("mscorlib.dll");
			compilerParams.ReferencedAssemblies.Add("System.dll");
			compilerParams.ReferencedAssemblies.Add("System.Windows.Forms.dll");
			compilerParams.ReferencedAssemblies.Add("NoxShared.dll");
			
			// Compile C# script
			CompilerResults results = csharp.CompileAssemblyFromFile(compilerParams, Directory.GetFiles(dirPath, "*.cs"));

			// Error handling
			if (results.Errors.Count > 0)
			{
				foreach (CompilerError e in results.Errors)
					System.Diagnostics.Debug.WriteLine(String.Format("{0} {1}: {2}", e.FileName, e.Line, e.ErrorText));
				return false;
			}

			// Save assembly in class variable
			scriptAssembly = results.CompiledAssembly;

			// Create Script Instance
			if (initScript())
			{
				// Run Script
				return runScript();
			}
			return false;
		}
		private bool initScript()
		{
			scriptClass = (BaseClass)scriptAssembly.CreateInstance(mInfo.name);
			return (scriptClass != null);
		}
		private bool runScript()
		{
			scriptClass.templateInfo = mInfo;
			scriptClass.Main();
			return true;
		}
		public class Info
		{
			public string name;
			public string author;
			public string description;
			public System.Windows.Forms.TabControl.TabPageCollection pages = null;
			public Dictionary<String,Object> xmlObjects = new Dictionary<string,object>();
			public WizardForm wizard = new WizardForm();
			public Map map;
            public Dictionary<Handler, System.Windows.Forms.TabPage> handlers = new Dictionary<Handler, System.Windows.Forms.TabPage>();
			public Info(Map map_l)
			{
				pages = ((System.Windows.Forms.TabControl)wizard.Controls["_oMainTabControl"]).TabPages;
                wizard.PageEnding += new EventHandler(Info_PageEnding);
                wizard.Controls["_oFinishBtn"].Click += new EventHandler(Info_Click);
                map = map_l;
			}

            void Info_Click(object sender, EventArgs e)
            {
                foreach (object o in xmlObjects.Values)
                    switch (o.GetType().FullName)
                    {
                        case "NoxShared.Map+Object":
                            map.Objects.Add((Map.Object)o);
                            break;
                        case "NoxShared.Map+Tile":
                            map.Tiles.Add(((Map.Tile)o).Location, (Map.Tile)o);
                            break;
                        case "NoxShared.Map+Wall":
                            map.Walls.Add(((Map.Wall)o).Location, (Map.Wall)o);
                            break;
                    }
            }

            void Info_PageEnding(object sender, EventArgs e)
            {
                foreach (KeyValuePair<Handler, System.Windows.Forms.TabPage> kvp in handlers)
                    if (kvp.Value == sender)
                        kvp.Key.eventHandler();
            }
		}
		public void Show()
		{
			mInfo.wizard.ShowDialog();
		}
		private void parseTemplateXml(string xmlFile)
		{
			System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(xmlFile);
			Type infoType = typeof(Template.Info);
			System.Reflection.FieldInfo fi = null;
			while (xmlReader.Read())
			{
				if (xmlReader.Name == "template")
					break;
				if (xmlReader.EOF)
				{
					xmlReader.Close();
					return;
				}
			}
			while (xmlReader.Read())
			{
				if (xmlReader.IsStartElement())
					if (!xmlReader.IsEmptyElement)
					{
						switch (xmlReader.Name.ToLower())
						{
							case "data":
								if (xmlReader.IsStartElement())
								{
									while (xmlReader.Read() && (xmlReader.Name.ToLower() != "data" || (xmlReader.Name.ToLower() == "data" && xmlReader.IsStartElement())))
									{
										if (xmlReader.Name.ToLower() == "object")
										{
											mInfo.xmlObjects.Add(xmlReader.GetAttribute("id"),XML.importXml(File.OpenRead(Path.GetDirectoryName(xmlFile) + Path.DirectorySeparatorChar + xmlReader.ReadElementContentAsString())));
										}
									}
								}
								break;
							case "pages":
								System.Windows.Forms.TabPage page = null;
								if (xmlReader.IsStartElement())
								{
									while (xmlReader.Read() && (xmlReader.Name.ToLower() != "pages" || (xmlReader.Name.ToLower() == "pages" && xmlReader.IsStartElement())))
									{
										switch(xmlReader.Name.ToLower())
										{
											case "page":
												if (xmlReader.IsStartElement())
												{
													page = new System.Windows.Forms.TabPage();
													mInfo.pages.Add(page);
												}
												else
													page = null;
												break;
											case "title":
												if (page != null)
													page.Text = xmlReader.ReadElementContentAsString();
												break;
											case "info":
												if (page != null)
												{
													// infoBox
													System.Windows.Forms.TextBox infoBox = new System.Windows.Forms.TextBox();
													infoBox.Location = new System.Drawing.Point(12, 9);
													infoBox.Multiline = true;
													infoBox.Name = "infoBox";
													infoBox.ReadOnly = true;
													infoBox.Size = new System.Drawing.Size(mInfo.wizard.Controls["_oMainTabControl"].Width - 24, 85);
													infoBox.TabIndex = 2;
													infoBox.Text = xmlReader.ReadElementContentAsString();
													page.Controls.Add(infoBox);
												}
												break;
											case "input":
												if (page != null)
												{
													System.Windows.Forms.Control controlElement = null;
                                                    String controlType = xmlReader.GetAttribute("type").ToLower();
													String controlName = xmlReader.GetAttribute("name");
													if (controlName != null && controlType != null)
													{
														switch (controlType)
														{
															case "text":
																controlElement = new System.Windows.Forms.TextBox();
																((System.Windows.Forms.TextBox)controlElement).Multiline = (xmlReader.GetAttribute("multiline") == "true");
																controlElement.Text = xmlReader.GetAttribute("default");
																break;
															case "combo":
																controlElement = new System.Windows.Forms.ComboBox();
																((System.Windows.Forms.ComboBox)controlElement).SelectedIndex = ((System.Windows.Forms.ComboBox)controlElement).Items.IndexOf(xmlReader.GetAttribute("default"));
																break;
															case "checkbox":
																controlElement = new System.Windows.Forms.CheckBox();
																break;
															case "button":
																controlElement = new System.Windows.Forms.Button();
																controlElement.Text = xmlReader.GetAttribute("text"); ;
																break;
														}
														if (controlElement != null)
														{
															controlElement.Name = controlName;
															controlElement.MaximumSize = mInfo.wizard.ClientSize;
															String location = xmlReader.GetAttribute("location");
															if(location != null)
																controlElement.Location = new System.Drawing.Point(Convert.ToInt32(location.Split(' ')[0]),Convert.ToInt32(location.Split(' ')[1]));
															String size = xmlReader.GetAttribute("size");
															if (size != null)
																controlElement.Size = new System.Drawing.Size(Convert.ToInt32(size.Split(' ')[0]), Convert.ToInt32(size.Split(' ')[1]));
															page.Controls.Add(controlElement);
                                                            while (xmlReader.Read() && (xmlReader.Name.ToLower() != "input" || (xmlReader.Name.ToLower() == "input" && xmlReader.IsStartElement())))
															{
                                                                switch (xmlReader.Name.ToLower())
                                                                {
                                                                    case "select":
                                                                        if(controlType == "combo")
                                                                            ((System.Windows.Forms.ComboBox)controlElement).Items.Add(xmlReader.ReadElementContentAsString());
                                                                        break;
                                                                    case "handler":
                                                                        Handler handler = new Handler();
                                                                        String temp;
                                                                        if (!xmlReader.HasAttributes)
                                                                            System.Windows.Forms.MessageBox.Show("Error! No attributes for handler!");
                                                                        handler.dataId = xmlReader.GetAttribute("id");
                                                                        temp = xmlReader.GetAttribute("overwrite");
                                                                        if(temp != null)
                                                                            handler.overwrite = temp.ToLower() == "true";
                                                                        temp = xmlReader.GetAttribute("type");
                                                                        if(temp != null)
                                                                            handler.dataType = System.Type.GetType(temp);
                                                                        handler.fieldName = xmlReader.ReadElementContentAsString();
                                                                        handler.mInfo = mInfo;
                                                                        handler.controlType = controlType;
                                                                        handler.control = controlElement;
                                                                        mInfo.handlers.Add(handler,page);
                                                                        break;
                                                                }
															}
														}
													}
												}
												break;
										}
									}
								}
								break;
							case "size":
								System.String[] sizeStringElement = xmlReader.ReadElementContentAsString().Split(' ');
								mInfo.wizard.Controls["_oMainTabControl"].Size = new System.Drawing.Size(Convert.ToInt32(sizeStringElement[0]), Convert.ToInt32(sizeStringElement[1]));
								mInfo.wizard.Size = new System.Drawing.Size(mInfo.wizard.Controls["_oMainTabControl"].Size.Width + 8, mInfo.wizard.Controls["_oMainTabControl"].Size.Height + 69);
								break;
							default:
								fi = infoType.GetField(xmlReader.Name);
								fi.SetValue(mInfo,xmlReader.ReadElementContentAs(fi.FieldType,null));
								break;
						}
					}
			}
			xmlReader.Close();
		}
	}
	public class BaseClass
	{
        public virtual void Main() { } // Contains init code, etc.
		public Template.Info templateInfo;
	}
    public class Handler
    {
        public String controlType;
        public System.Windows.Forms.Control control;
        public String dataId;
        public String fieldName;
        public Template.Info mInfo;
        private FieldInfo fInfo = null;

        /* if field = Byte[] */
        public Int16 position = 0;
        public Boolean overwrite = false;
        public Type dataType;
        /* end if */

        public void eventHandler()
        {
            if (fInfo == null)
                GetFieldInfo();
            if (fInfo == null)
                return;
            String text = control.Text;
            /*switch (controlType)
            {
                case "text":
                    System.Windows.Forms.TextBox textbox = (System.Windows.Forms.TextBox)control;
                    fInfo.SetValue(mInfo.xmlObjects[dataId], System.Convert.ChangeType(textbox.Text, fInfo.FieldType));
                    break;
                case "combo":
                    System.Windows.Forms.ComboBox combobox = (System.Windows.Forms.ComboBox)control;
                    fInfo.SetValue(mInfo.xmlObjects[dataId], System.Convert.ChangeType(combobox.Text, fInfo.FieldType));
                    break;
                case "checkbox":
                    System.Windows.Forms.CheckBox checkbox = (System.Windows.Forms.CheckBox)control;
                    fInfo.SetValue(mInfo.xmlObjects[dataId], System.Convert.ChangeType(checkbox.Text, fInfo.FieldType));
                    break;
            }*/
            switch (fInfo.FieldType.FullName)
            {
                case "System.Drawing.PointF":
                    fInfo.SetValue(mInfo.xmlObjects[dataId], new System.Drawing.PointF(Convert.ToSingle(text.Split(' ')[0]), Convert.ToSingle(text.Split(' ')[1])));
                    break;
                case "System.Byte[]":
                    if(dataType != null)
                    {
                        Byte[] current = (Byte[])fInfo.GetValue(mInfo.xmlObjects[dataId]);
                        if (position > current.Length)
                            position = (short)current.Length;
                        System.IO.MemoryStream memstream = new System.IO.MemoryStream();
                        System.IO.BinaryWriter wtr = new System.IO.BinaryWriter(memstream);
                        if (!overwrite)
                        {
                            wtr.Write(current, 0, position - 1);
                            switch (dataType.FullName)
                            {
                                case "System.Int32":
                                    wtr.Write((Int32)System.Convert.ToInt32(text));
                                    break;
                                case "System.Byte":
                                    wtr.Write((Byte)System.Convert.ToByte(text));
                                    break;
                                case "System.Int16":
                                    wtr.Write((Int16)System.Convert.ToInt16(text));
                                    break;
                                case "System.Int64":
                                    wtr.Write((Int64)System.Convert.ToInt64(text));
                                    break;
                            }
                            wtr.Write(current, position, current.Length - position);
                        }
                        else
                        {
                            wtr.Write(current);
                            wtr.Seek(position, SeekOrigin.Begin);
                            switch (dataType.FullName)
                            {
                                case "System.Int32":
                                    wtr.Write((Int32)System.Convert.ToInt32(text));
                                    break;
                                case "System.Byte":
                                    wtr.Write((Byte)System.Convert.ToByte(text));
                                    break;
                                case "System.Int16":
                                    wtr.Write((Int16)System.Convert.ToInt16(text));
                                    break;
                                case "System.Int64":
                                    wtr.Write((Int64)System.Convert.ToInt64(text));
                                    break;
                            }
                        }
                        fInfo.SetValue(mInfo.xmlObjects[dataId], memstream.ToArray());
                        wtr.Close();
                    }
                    break;
                case "NoxShared.Map+Object+Property":
                    fInfo.SetValue(mInfo.xmlObjects[dataId], System.Convert.ToInt16(text));
                    break;
                case "System.Collections.ArrayList":
                    break;
                case "System.Collections.Generic.List<>":
                    break;
                default:
                    fInfo.SetValue(mInfo.xmlObjects[dataId], System.Convert.ChangeType(text, fInfo.FieldType));
                    break;
            }
        }
        private void GetFieldInfo()
        {
            fInfo = mInfo.xmlObjects[dataId].GetType().GetField(fieldName, BindingFlags.IgnoreCase | BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic);
        }
    }
}
