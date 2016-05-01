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

        private static SqlLiteHelper db = null;

        public SqlLiteInstance(SqlLiteHelper sqlInstanceHelper)
        {
            db = sqlInstanceHelper;
        }

        public void InitTables()
        {
            db.CreateTable<Language>();
            LanguageManager langManager = new LanguageManager(db);
            langManager.InitDefaultData();

            db.CreateTable<TranslateProvider>();

            db.CreateTable<Direction>();
            DirectionManager managerDirection = new DirectionManager(db);
            managerDirection.InitDefaultData();

            db.CreateTable<TranslateProvider>();
            TranslateProviderManager managerProvider = new TranslateProviderManager(db);
            managerProvider.InitDefaultData();

            db.CreateTable<Favorites>();

            db.CreateTable<SourceExpression>();
            //SourceExpressionManager managerSourceExpression = new SourceExpressionManager(db);
            //managerSourceExpression.InitDefaultData();

            db.CreateTable<TranslatedExpression>();

            db.CreateTable<DefinitionTypes>();
            DefinitionTypesManager managerTypes = new DefinitionTypesManager(db);
            managerTypes.InitDefaultData();

            db.CreateTable<SourceDefinition>();

            db.CreateTable<Chats>();

            db.CreateTable<ChatHistory>();

        }

    }
}