using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Domain.Entities;
using System.Linq;

namespace Domain.Data
{
    public partial class SahaflarPazari : DbContext
    {
        public SahaflarPazari()
            : base("name=SahaflarPazari")
        {
        }

        public virtual DbSet<Adres> Adres { get; set; }
        public virtual DbSet<AlisverisSepeti> AlisverisSepetis { get; set; }
        public virtual DbSet<IstekListesi> IstekListesis { get; set; }
        public virtual DbSet<Kitap> Kitaps { get; set; }
        public virtual DbSet<KitapKategorileri> KitapKategorileris { get; set; }
        public virtual DbSet<KitapResimleri> KitapResimleris { get; set; }
        public virtual DbSet<Kullanici> Kullanicis { get; set; }
        public virtual DbSet<KullaniciBilgileri> KullaniciBilgileris { get; set; }
        public virtual DbSet<Roller> Rollers { get; set; }
        public virtual DbSet<Sikayet> Sikayets { get; set; }
        public virtual DbSet<Siparisler> Siparislers { get; set; }
        public virtual DbSet<Yayinevi> Yayinevis { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adres>()
                .Property(e => e.AdresAdi)
                .IsUnicode(false);

            modelBuilder.Entity<Adres>()
                .Property(e => e.Sehir)
                .IsUnicode(false);

            modelBuilder.Entity<Adres>()
                .Property(e => e.Ilce)
                .IsUnicode(false);

            modelBuilder.Entity<Adres>()
                .Property(e => e.Mahalle)
                .IsUnicode(false);

            modelBuilder.Entity<Adres>()
                .Property(e => e.PostaKodu)
                .IsUnicode(false);

            modelBuilder.Entity<Adres>()
                .Property(e => e.AdresAlani)
                .IsUnicode(false);

            modelBuilder.Entity<Kitap>()
                .Property(e => e.KitapAdi)
                .IsUnicode(false);

            modelBuilder.Entity<Kitap>()
                .Property(e => e.Detay)
                .IsUnicode(false);

            modelBuilder.Entity<Kitap>()
                .Property(e => e.Yazar)
                .IsUnicode(false);

            modelBuilder.Entity<Kitap>()
                .HasMany(e => e.AlisverisSepetis)
                .WithRequired(e => e.Kitap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kitap>()
                .HasMany(e => e.KitapResimleris)
                .WithRequired(e => e.Kitap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kitap>()
                .HasMany(e => e.Sikayets)
                .WithRequired(e => e.Kitap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kitap>()
                .HasMany(e => e.Siparislers)
                .WithRequired(e => e.Kitap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KitapKategorileri>()
                .Property(e => e.KategoriAdi)
                .IsUnicode(false);

            modelBuilder.Entity<KitapKategorileri>()
                .HasMany(e => e.Kitaps)
                .WithRequired(e => e.KitapKategorileri)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KitapResimleri>()
                .Property(e => e.DataYolu)
                .IsUnicode(false);

            modelBuilder.Entity<Kullanici>()
                .Property(e => e.KullaniciAdi)
                .IsUnicode(false);

            modelBuilder.Entity<Kullanici>()
                .Property(e => e.Sifre)
                .IsUnicode(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Adres)
                .WithRequired(e => e.Kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.AlisverisSepetis)
                .WithRequired(e => e.Kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.IstekListesis)
                .WithRequired(e => e.Kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Kitaps)
                .WithRequired(e => e.Kullanici)
                .HasForeignKey(e => e.SaticiId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasOptional(e => e.KullaniciBilgileri)
                .WithRequired(e => e.Kullanici);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Rollers)
                .WithRequired(e => e.Kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Sikayets)
                .WithRequired(e => e.Kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Siparislers)
                .WithRequired(e => e.Kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KullaniciBilgileri>()
                .Property(e => e.Ad)
                .IsUnicode(false);

            modelBuilder.Entity<KullaniciBilgileri>()
                .Property(e => e.Soyad)
                .IsUnicode(false);

            modelBuilder.Entity<KullaniciBilgileri>()
                .Property(e => e.Telefon)
                .IsUnicode(false);

            modelBuilder.Entity<KullaniciBilgileri>()
                .Property(e => e.Eposta)
                .IsUnicode(false);

            modelBuilder.Entity<Roller>()
                .Property(e => e.RolAdi)
                .IsUnicode(false);

            modelBuilder.Entity<Sikayet>()
                .Property(e => e.SikayetIcerigi)
                .IsUnicode(false);

            modelBuilder.Entity<Siparisler>()
                .Property(e => e.Adres)
                .IsUnicode(false);

            modelBuilder.Entity<Siparisler>()
                .Property(e => e.SiparisDurumu)
                .IsUnicode(false);
        }
    }
}
