namespace Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryAndPublisherInitialAdding : DbMigration
    {
        public override void Up()
        {
            AddCategory("Roman");
            AddCategory("Hikaye");
            AddCategory("Şiir");
            AddCategory("Tarih");
            AddCategory("Bilim");
            AddCategory("Felsefe");
            AddCategory("Din");
            AddCategory("Edebiyat");
            AddCategory("Bilgisayar");
            AddCategory("Çocuk");
            AddCategory("Dergi");
            AddCategory("Diğer");

            AddPublisher("İş Bankası");
            AddPublisher("Can Yayınları");
            AddPublisher("Doğan Kitap");
            AddPublisher("Yapı Kredi Yayınları");
            AddPublisher("Everest Yayınları");
            AddPublisher("Timaş Yayınları");
            AddPublisher("Kırmızı Kedi Yayınları");
            AddPublisher("Pegasus Yayınları");
            AddPublisher("İthaki Yayınları");
            AddPublisher("Alfa Yayınları");
            AddPublisher("Diğer");
        }
        
        public override void Down()
        {
            RemoveCategory("Roman");
            RemoveCategory("Hikaye");
            RemoveCategory("Şiir");
            RemoveCategory("Tarih");
            RemoveCategory("Bilim");
            RemoveCategory("Felsefe");
            RemoveCategory("Din");
            RemoveCategory("Edebiyat");
            RemoveCategory("Bilgisayar");
            RemoveCategory("Çocuk");
            RemoveCategory("Dergi");
            RemoveCategory("Diğer");

            RemovePublisher("İş Bankası");
            RemovePublisher("Can Yayınları");
            RemovePublisher("Doğan Kitap");
            RemovePublisher("Yapı Kredi Yayınları");
            RemovePublisher("Everest Yayınları");
            RemovePublisher("Timaş Yayınları");
            RemovePublisher("Kırmızı Kedi Yayınları");
            RemovePublisher("Pegasus Yayınları");
            RemovePublisher("İthaki Yayınları");
            RemovePublisher("Alfa Yayınları");
            RemovePublisher("Diğer");

        }
        private void AddCategory(string categoryName)
        {
            Sql($"INSERT INTO BookCategory (CategoryName) VALUES ('{categoryName}')");
        }
        private void RemoveCategory(string categoryName)
        {
            Sql($"DELETE FROM BookCategory WHERE CategoryName = '{categoryName}'");
        }
        private void AddPublisher(string publisherName)
        {
            Sql($"INSERT INTO Publisher (PublisherName) VALUES ('{publisherName}')");
        }
        private void RemovePublisher(string publisherName)
        {
            Sql($"DELETE FROM Publisher WHERE PublisherName = '{publisherName}'");
        }
    }
}
