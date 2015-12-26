using PortableCore.BL.Managers;
using PortableCore.Core.DL;
using PortableCore.DL;

namespace PortableCore.Core.DAL
{
    public class SqlLiteInstance
    {
        public static SqlLiteHelper DB
        {
            get
            {
                return db;
            }
        }

        private static PortableCore.Core.DL.SqlLiteHelper db = null;

        public SqlLiteInstance(SqlLiteHelper sqlInstanceHelper)
        {
            db = sqlInstanceHelper;
        }

        public void InitTables()
        {
            db.CreateTable<Language>();
            db.CreateTable<TranslateProvider>();
            db.CreateTable<Direction>();
            db.CreateTable<TranslateProvider>();
            db.CreateTable<Favorites>();
            db.CreateTable<SourceExpression>();
            db.CreateTable<TranslatedExpression>();
            db.CreateTable<DefinitionTypes>();

            DefinitionTypesManager managerTypes = new DefinitionTypesManager();
            managerTypes.InitDefaultData();

            TranslateProviderManager managerProvider = new TranslateProviderManager();
            managerProvider.InitDefaultData();
        }

    }
    /*Core.DefinitionTypesManager managerTypes = new Core.DefinitionTypesManager();
managerTypes.InitDefaultData ();

Core.TranslateProviderManager managerProvider = new Core.TranslateProviderManager ();
managerProvider.InitDefaultData ();*/
}