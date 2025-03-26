using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace sELedit.CORE.BASE.CONTROLLERS
{
	public class CategoryControlManager
	{
		private readonly Dictionary<string[], string> categoryMappings = new Dictionary<string[], string>
	{
		{ new[] { "probability_addon", "addons_", "skills_", "after_death", "skill_hp" }, "addons" },
		{ new[] { "skills_","skill_hp" }, "skills" },
		{ new[] { "rands_" }, "rands" },
		{ new[] { "uniques_" }, "uniques" },
		{ new[] { "materials_", "drop_matters_" }, "materials" },
		{ new[] { "drop_probability", "make_probability" }, "drop/make" }
	};

		private readonly Dictionary<string, bool> tabStatus = new Dictionary<string, bool>
	{
		{ "addons", false },
		{ "skills", false },
		{ "rands", false },
		{ "uniques", false },
		{ "materials", false },
		{ "drop/make", false },
		{ "default", false }
	};

		public Dictionary<string, List<object[]>> GroupElementValuesByCategory(eList list)
		{
			
			var groupedValues = list.elementFields
				.Select((field, index) => new { Field = field, Index = index }) 
				.GroupBy(e => GetCategory(e.Field))
				.ToDictionary(
					g => g.Key,
					g => g.Select(e => list.elementValues.Select(row => row[e.Index]).ToArray()).ToList()
				);

			return groupedValues;
		}

		private string GetCategory(string field)
		{
			
			foreach (var mapping in categoryMappings)
			{
				if (mapping.Key.Any(prefix => field.StartsWith(prefix)))
					return mapping.Value;  
			}
			return "default"; 
		}

		public List<Control> CreateControlsFromGroupedValues(Dictionary<string, List<object[]>> groupedValues)
		{
			var controls = new List<Control>();

			// Criar botões baseados nas categorias agrupadas
			foreach (var group in groupedValues)
			{
				string category = group.Key.ToUpper();
				
				controls.Add(CreateButton($"btn_{category}", category));
			}

			return controls;
		}




















		// Método para verificar categorias e retornar controles
		public List<Control> GenerateControls(string input)
		{
			var controls = new List<Control>();

			var matchedCategory = categoryMappings.FirstOrDefault(entry => entry.Key.Any(input.StartsWith)).Value;

			if (matchedCategory != null && !tabStatus[matchedCategory])
			{
				tabStatus[matchedCategory] = true; // Marca a aba como ativada
				controls.Add(CreateButton(input, matchedCategory.ToUpper()));
			}
			else if (matchedCategory == null) // Caso seja PRIMAL
			{
				controls.AddRange(HandlePrimalCase(input));
			}

			return controls;
		}

		private IEnumerable<Control> HandlePrimalCase(string input)
		{
			var controls = new List<Control>();
			if (!tabStatus["PRIMAL"])
			{
				tabStatus["PRIMAL"] = true;
				controls.Add(CreateButton("PRIMAL", "PRIMAL"));
			}

			// Pode adicionar mais lógica aqui, caso necessário
			return controls;
		}

		// Cria o botão para o título (personalize conforme necessário)
		//private Button CreateButton(string name, string text)
		//{
		//	return new Button
		//	{
		//		Name = $"btn_{name}",
		//		Text = text,
		//		Width = 100,
		//		Height = 30
		//	};
		//}

		private Button CreateButton(string NAME, string TXT)
		{
			Button button_PRIMAL = new Button();
			button_PRIMAL.BackgroundImage = global::sELedit.Properties.Resources.tab_off;
			button_PRIMAL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			button_PRIMAL.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
			button_PRIMAL.FlatAppearance.BorderSize = 0;
			button_PRIMAL.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			button_PRIMAL.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
			button_PRIMAL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			button_PRIMAL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			button_PRIMAL.ForeColor = System.Drawing.Color.Goldenrod;
			button_PRIMAL.Location = new System.Drawing.Point(3, 3);
			button_PRIMAL.Name = NAME;
			button_PRIMAL.Size = new System.Drawing.Size(148, 47);
			button_PRIMAL.TabIndex = 0;
			button_PRIMAL.Text = TXT;
			button_PRIMAL.Cursor = AdvancedCursorsFromEmbededResources.Create(Properties.Resources.Hand);
			button_PRIMAL.UseVisualStyleBackColor = true;
			//button_PRIMAL.MouseEnter += new System.EventHandler(MouseEnter);
			//button_PRIMAL.MouseLeave += new System.EventHandler(MouseLeave);
			//button_PRIMAL.Click += new System.EventHandler(MouseClick);

			return button_PRIMAL;
		}



		//public void Create(eList list)
		//{

		//	var grouped = lists
		//   .GroupBy(e => e.listName.Split('_').Last())
		//   .ToDictionary(g => g.Key, g => g.ToList());



		//}
	}
}
