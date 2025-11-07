# 🧩 MVVM-projekt – Struktur, Analys och Reflektion

## 📖 Sammanfattning – Struktur och uppbyggnad

Detta projekt är ett första försök att tillämpa en **MVVM-arkitektur** (Model–View–ViewModel).  
Strukturen är uppbyggd enligt följande principer:

- **Views** innehåller endast *markup*-kod för att skapa den grafiska strukturen för användargränssnittet (UI).  
  Här definieras även bindningar till händelser som initieras av användaren, till exempel knapptryckningar, inmatningar och val i listor.
  
- I tillhörande **code-behind** skapas samarbete med motsvarande **ViewModel** genom att den instansieras och tilldelas som *DataContext*.  
  Här definieras också hur resultat från händelser ska tas emot och vad som ska ske när de inträffar.  
  Exempelvis kan information tas emot när en inloggning lyckas, och då utförs specifika åtgärder.  
  Detta sker genom så kallade *prenumerationer* på händelser (*events*).

- I **ViewModels** definieras kommandon och metodkommandon för de händelser som triggas via användarens aktiviteter.  
  Dessa kommandon kan i sin tur skicka information vidare till olika **Managers**.

- **Managers** hanterar logiken bakom datan. I detta projekt används de främst som struktur, men i en fullständig implementation skulle de bland annat ansvara för kommunikation med databaser och liknande källor.  
  De arbetar även mot **Models**, som innehåller definitioner av grundläggande entiteter — till exempel mallar för recept och olika typer av användare.

- Utöver dessa finns två **basklasser/hjälpklasser** för händelsehantering, samt en som definierar grundläggande funktionalitet för kommandon.

- I **App.xaml** deklareras global (programövergripande) information, såsom delade resurser (`UserManager` och `RecipeManager`).  
  I den tillhörande *code-behind*-filen beskrivs vad som ska hända vid programstart (t.ex. vilket fönster som ska öppnas först) och hur oväntade avstängningar kan förebyggas.

## ⚙️ Analys – Fördelar, nackdelar och reflektioner

**Fördelar med MVVM-arkitekturen:**
- Databindning erbjuder omedelbara uppdateringar, vilket förbättrar användarupplevelsen.  
- Kodens uppdelning i separata lager underlättar felsökning, underhåll och återanvändning.  
- Klientsidan blir lättare, vilket minskar belastningen på servern.  
- Applikationen blir mer skalbar – den kan växa eller krympa utan att UI:t påverkas.  
- Känslig information hålls längre bort från användaren, vilket förbättrar säkerheten.

## 💡 Personliga lärdomar och framtida förbättringar

Som en person som gärna *gör först och tänker sen* har det varit en utmaning att förstå MVVM-mönstret samtidigt som jag började koda projektet.  
Jag har lärt mig att det är viktigt att **stanna upp i början av ett projekt**, reflektera över arkitekturen och planera strukturen innan man börjar skriva kod.

Jag inspireras av kollegor som haft mer strukturerade tillvägagångssätt och ser fram emot att fortsätta utvecklas inom MVVM.  
I jämförelse med andra arkitekturmönster som **MVC** (Model–View–Controller) och **MVP** (Model–View–Presenter), upplever jag att MVVM erbjuder en mer komplett lösning.

Jag är medveten om att koden som nu levereras kan förbättras ytterligare och ser fram emot **konstruktiv och pedagogisk återkoppling**.  
Framöver vill jag särskilt titta på:

- ⚙️ Att minska onödiga instansieringar av *Managers* och fönster för en renare kodbas.  
- ⚡ Att se över olika händelser (*events*) för att effektivisera koden.  
- 🧠 Att fördjupa min förståelse för databindning och kommandohantering i MVVM.

## 📌 Sammanfattande tankar

Detta projekt är ett första steg mot att fullt ut tillämpa MVVM-arkitekturen i praktiken.  
Även om vissa delar kan förbättras, har arbetet gett en solid grund för vidare utveckling och förståelse för hur man bygger strukturerade, skalbara och användarvänliga WPF-applikationer.

## 🤝 Tack till lärare och studiekolleger för tips och support! 

✍️ *Författare:* Linda-Maria Modig  
📅 *Datum:* 2025-11-07
📁 *Projekt:* Cook_Master_för_MVVM-Implementering
