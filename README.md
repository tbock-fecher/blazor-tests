# blazor-tests – Calculator Demo

Kleine Blazor Server App mit dem portierten WPF-Rechner, erweitert um ein verschiebbares Fenster und Tastatursteuerung.

## Einrichten & Starten
- Voraussetzungen: .NET SDK 8.0+
- Kommandos (aus `CalculatorDemo.Blazor`):
  - `dotnet restore`
  - `dotnet run`
- Browser öffnet sich auf http://localhost:5000 (oder https).

## Bedienung
- Drag & Drop: Rechnerkopf anfassen und ziehen; bleibt im Viewport, auch bei Resize.
- Tastatur-Shortcuts:
  - Ziffern `0-9`, Dezimaltrennzeichen `.` oder `,`
  - Operatoren `+ - * / %`
  - `Enter` oder `=` = Ergebnis
  - `Backspace` = letzte Eingabe löschen
  - `S` = Wurzel, `R` = 1/x, `N` = Vorzeichen wechseln
  - `C` oder `Esc` = Alles löschen

## Wichtige Dateien
- `Pages/Index.razor` – UI und JS-Interop für Drag & Tastatur
- `wwwroot/js/calculator-drag.js` – Drag-Logik & Keydown-Layer
- `wwwroot/css/site.css` – Layout/Styles des Rechners

## Entstehungs-Prompts
1. "Ich würde gerne die Demo-Application Calculator Demo von WPF zu Blazor umstellen"
2. "Ziel ist erstmal eine Server-Application und ja, bitte führe die Befehle aus"
3. "Bitte das KeyPad in der Anordnung analog der WPF-Application anordnen"
4. "Bitte das Fenster der Calculators so umbauen, dass ich es verschieben kann"
5. "Bitte Tastatureingaben auch ermöglichen"
6. "Bitte die Readme.md ausfüllen, vor allem mit allen Erstellungs-Promts bis zu diesem Zeitpunkt"
