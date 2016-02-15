using System;
using System.Collections.Generic;
using System.Linq;
using PortableCore.BL.Contracts;
using PortableCore.DL;

namespace PortableCore.BL.Managers
{
	public class DefinitionTypesManager : IDataManager<DefinitionTypes>
	{
		ISQLiteTesting db;

		public DefinitionTypesManager (ISQLiteTesting dbHelper)
		{
			db = dbHelper;
		}

		public void InitDefaultData ()
		{
			DAL.Repository<DefinitionTypes> repos = new DAL.Repository<DefinitionTypes> ();
			DefinitionTypes[] data = getDefaultData ();
			var currentData = GetItems ();
			if (currentData.Count != data.Length) {
				repos.DeleteAllDataInTable ();
				repos.AddItemsInTransaction (data);
			}
		}

		public List<DefinitionTypes> GetItems ()
		{
			DAL.Repository<DefinitionTypes> repos = new DAL.Repository<DefinitionTypes> ();
			return new List<DefinitionTypes> (repos.GetItems ());
		}

		public DefinitionTypes GetItemForId (int id)
		{
			DAL.Repository<DefinitionTypes> repos = new DAL.Repository<DefinitionTypes> ();
			DefinitionTypes result = repos.GetItem (id);
			return result;
		}

		private DefinitionTypes[] getDefaultData ()
		{
			DefinitionTypes[] defTypesList = new DefinitionTypes[] {
				new DefinitionTypes (){ Name = "noun", ID = (int)GetEnumDefinitionTypeFromName ("noun") }, 
				new DefinitionTypes (){ Name = "verb", ID = (int)GetEnumDefinitionTypeFromName ("verb") },
				new DefinitionTypes (){ Name = "adjective", ID = (int)GetEnumDefinitionTypeFromName ("adjective") },
				new DefinitionTypes (){ Name = "adverb", ID = (int)GetEnumDefinitionTypeFromName ("adverb") },
				new DefinitionTypes (){ Name = "participle", ID = (int)GetEnumDefinitionTypeFromName ("participle") },
				new DefinitionTypes (){ Name = "predicative", ID = (int)GetEnumDefinitionTypeFromName ("predicative") },
				new DefinitionTypes (){ Name = "numeral", ID = (int)GetEnumDefinitionTypeFromName ("numeral") },
				new DefinitionTypes (){ Name = "particle", ID = (int)GetEnumDefinitionTypeFromName ("particle") },
				new DefinitionTypes (){ Name = "pronoun", ID = (int)GetEnumDefinitionTypeFromName ("pronoun") },
				new DefinitionTypes (){ Name = "translater", ID = (int)GetEnumDefinitionTypeFromName ("translater") },
                new DefinitionTypes (){ Name = "preposition", ID = (int)GetEnumDefinitionTypeFromName ("preposition") },
                new DefinitionTypes (){ Name = "conjunction", ID = (int)GetEnumDefinitionTypeFromName ("conjunction") },
                new DefinitionTypes (){ Name = "interjection", ID = (int)GetEnumDefinitionTypeFromName ("interjection") },
                new DefinitionTypes (){ Name = "parenthetic", ID = (int)GetEnumDefinitionTypeFromName ("parenthetic") },                
            };
			return defTypesList;
		}

		/// <summary>
		/// возвращает наименование перечисления на кириллице
		/// </summary>
		/// <param name="typeEnum"></param>
		/// <returns></returns>
		public static string GetRusNameForEnum (DefinitionTypesEnum typeEnum)
		{
			//ToDo:наверняка можно сделать через ресурсы, надо разобраться как
			string result = string.Empty;
			switch (typeEnum) {
			case DefinitionTypesEnum.adjective:
				{
					result = "прилагательное";
				}
				;
				break;
			case DefinitionTypesEnum.adverb:
				{
					result = "наречие";
				}
				;
				break;
			case DefinitionTypesEnum.noun:
				{
					result = "существительное";
				}
				;
				break;
			case DefinitionTypesEnum.participle:
				{
					result = "причастие";
				}
				;
				break;
			case DefinitionTypesEnum.verb:
				{
					result = "глагол";
				}
				;
				break;
			case DefinitionTypesEnum.predicative:
				{
					result = "предикат";
				}
				;
				break;
			case DefinitionTypesEnum.numeral:
				{
					result = "числительное";
				}
				;
				break;
			case DefinitionTypesEnum.particle:
				{
					result = "частица";
				}
				;
				break;
			case DefinitionTypesEnum.pronoun:
				{
					result = "местоимение";
				}
				;
				break;
			case DefinitionTypesEnum.translater:
				{
					result = "перевод предложения";
				}
				;
				break;
			case DefinitionTypesEnum.preposition:
				{
					result = "предлог";
				}
				;
				break;
			case DefinitionTypesEnum.conjunction:
				{
					result = "союз";
				}
				;
				break;
            case DefinitionTypesEnum.interjection:
                {
                    result = "междометие";
                }
                ;
                break;
            case DefinitionTypesEnum.parenthetic:
                {
                    result = "вводное слово";
                }
                ;
                    break;
                case DefinitionTypesEnum.unknown:
			default:
				{
					result = "неизвестная часть речи";
				}
				;
				break;
			}

			return result;
		}

		public static DefinitionTypesEnum GetEnumDefinitionTypeFromName (string name)
		{
			DefinitionTypesEnum result = DefinitionTypesEnum.unknown;
            
            switch (name.ToLower ()) {
			case "союз":
			case "соединение":
			case "conjunction":
				{
					result = DefinitionTypesEnum.conjunction;
				}
				;
				break;
			case "существительное":
			case "noun":
				{
					result = DefinitionTypesEnum.noun;
				}
				;
				break;
			case "предлог":
			case "preposition":
				{
					result = DefinitionTypesEnum.preposition;
				}
				;
				break;
			case "глагол":
			case "verb":
				{
					result = DefinitionTypesEnum.verb;
				}
				;
				break;                    
			case "прилагательное":
			case "adjective":
				{
					result = DefinitionTypesEnum.adjective;
				}
				;
				break;
			case "наречие":
			case "adverb":
				{
					result = DefinitionTypesEnum.adverb;
				}
				;
				break;
			case "причастие":
			case "participle":
				{
					result = DefinitionTypesEnum.participle;
				}
				;
				break;
			case "предикат":
			case "predicative":
				{
					result = DefinitionTypesEnum.predicative;
				}
				;
				break;
			case "частица":
			case "particle":
				{
					result = DefinitionTypesEnum.particle;
				}
				;
				break;
			case "числительное":
			case "numeral":
				{
					result = DefinitionTypesEnum.numeral;
				}
				;
				break;
			case "местоимение":
			case "pronoun":
				{
					result = DefinitionTypesEnum.pronoun;
				}
				;
				break;
			case "перевод предложения":
			case "translater":
				{
					result = DefinitionTypesEnum.translater;
				}
				;
				break;
            case "междометие":
            case "interjection":
                {
                    result = DefinitionTypesEnum.interjection;
                }
            ;
                break;
                case "вводное слово":                    
                case "parenthetic":
                    {
                        result = DefinitionTypesEnum.parenthetic;
                    }
            ;
                    break;
                default:
				{
				}
				;
				break;
			}
			return result;
		}
	}
}

