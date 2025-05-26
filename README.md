# IkalaskuriVersio2.0

Lasketaan erotus tänään ja odotetun kuolinpäivän välillä.

TimeSpan erotus = kuolinpaiva - tanaan;

/* Tässä lasketaan montako kokonaista vuotta jäljellä olevista päivistä saadaan
   * erotus.Days on esim. 5000 päivää
   * Jaetaan 365.25:llä, koska vuodessa on noin 365.25 päivää (karkausvuodet huomioiden)
   * (int) pakottaa tuloksen kokonaisluvuksi (eli pyöristää alaspäin automaattisesti)
   
   Esimerkki:
   5000/365.25 = 13.69
   (int)13.69 -> 13 vuotta
   Huom: Desimaaliosa (0.69) häviää kokonaisluvuksi muuntamisessa*/
int vuodet = (int)(erotus.Days / 365.25);

/* Lasketaan, montako päivää jää vielä jäljelle, kun täydet vuodet on ensin otettu pois
   * vuodet * 365.25 kertoo kuinka monta päivää täydet vuodet veivät
   * Vähennetään tämä alkuperäisistä kokonaispäivistä
   
   Esimerkki jatkuu:
   13 vuotta * 365.25 = 4748 päivää
   5000 - 4748 = 252 päivää jää jäljelle*/
int paivatJaljella = erotus.Days - (int)(vuodet * 365.25);

/* Nyt jäljelle jääneet päivät jaetaan kuukausiksi
   * Oletetaan, että 1 kuukausi = 30 päivää(likimääräinen arvio)
   * Jaetaan suoraan 30:llä ja saadaan, montako kokonaista kuukautta mahtuu
   
   Esimerkissä:
   252/30 = 8 kuukautta (koska 8 * 30 = 240)*/
int kuukaudet = paivatJaljella / 30;

/* Lopuksi katsotaan, montako päivää jää vielä jäljelle, kun kuukaudet on otettu pois
   * % 30 tarkoittaa jakojäännöstä
   * Se kertoo, paljonko jää yli 30 päivän jaosta
   
   Esimerkissä:
   252 % 30 = 12 päivää*/

   /* Esimerkki käytännössä:
      Lasketaan 252 % 30
   
      1. 252/30 = 8.4
      2. Kokonaisosa = 8
      3. 8 * 30 = 240
      4. 252 -240 = 12
      Jakojäännös on 12
   
int paivat = paivatJaljella % 30;
