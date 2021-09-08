
1) CreatedByName boş geçilemez diyor ve CreatedDate'i her update işleminde yeniliyor.
	Çözüldü : BaseEntity'de Create Date yazmışım typo hatası yapmışım

2) Movie eklerken Actorlerini seçemiyorum. Aynı şekilde Actor seçerken de aynı sıkıntı var.
	Fikir : HttpPatch yöntemiyle movie'yi ekledikten sonra diğer entityleri ekleyeceğim
	Çözüldü : Yukardaki çözüm işe yaradı

3) Duplicate ekleme sorununu hallet
4) Rating null gelince patlıyor
5) Auto Updater yazılacak python
6) Domaine servisi yükle
7) DateTimeString başımıza bela olabilir onu da çözmemiz lazım
8) ReverseProxy ile çalışırken belki bu türkçe karakter sorunu çözülür
9) MigrationAssembly hatası veriyordu
	Çözüm : Startup.cs deki DbContext servisi ekleme ayarlarına b=>b.MigrationsAssembly("MovieService.Api") ekleyince sorun ortadan kalktı
// posgres sql connection string 
	database name : movie_db
	user : movie_db_user
	pass : 0410


sql server açık kalmış npql server açık duracaktı, connectionstringi gözden geçir bir de https sunucusuna bir bak.