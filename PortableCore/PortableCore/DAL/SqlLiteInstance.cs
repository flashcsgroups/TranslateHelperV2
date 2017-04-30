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

        public void InitTables(string libraryPath)
        {
            db.CreateTable<Language>();
            LanguageManager langManager = new LanguageManager(db);
            langManager.InitDefaultData();

            db.CreateTable<TranslateProvider>();

            db.CreateTable<TranslateProvider>();
            TranslateProviderManager managerProvider = new TranslateProviderManager(db);
            managerProvider.InitDefaultData();

            db.CreateTable<SourceExpression>();

            db.CreateTable<TranslatedExpression>();

            db.CreateTable<DefinitionTypes>();
            DefinitionTypesManager managerTypes = new DefinitionTypesManager(db);
            managerTypes.InitDefaultData();

            db.CreateTable<SourceDefinition>();

            db.CreateTable<Chat>();

            db.CreateTable<ChatHistory>();

            db.CreateTable<Anecdote>();
            AnecdoteManager managerAnecdotes = new AnecdoteManager(db, langManager);
            managerAnecdotes.LibraryPath = libraryPath;
            managerAnecdotes.InitDefaultData();
        }

    }
}