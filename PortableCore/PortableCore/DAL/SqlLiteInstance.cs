using PortableCore.BL.Managers;
using PortableCore.DL;

namespace PortableCore.DAL
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

        private static PortableCore.DL.SqlLiteHelper db = null;

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
            db.CreateTable<SourceDefinition>();

            DefinitionTypesManager managerTypes = new DefinitionTypesManager(db);
            managerTypes.InitDefaultData();

            TranslateProviderManager managerProvider = new TranslateProviderManager(db);
            managerProvider.InitDefaultData();
        }

    }
}