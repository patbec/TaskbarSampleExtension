# Taskbar Beispiel Erweiterung

![Screenshot Taskbar](https://raw.githubusercontent.com/patbec/TaskbarSampleExtension/master/screenshot-taskbar-sample.png)

### Beschreibung

Mit diesem Beispiel können DeskBand Erweiterungen für Windows 10 erstellt werden, dazu implementiert das Projekt die IDeskBand2 Schnittstelle.
Es gibt eine Unterstützung für Vertikale Taskbars, Gripper und Transparenz.

### Installation der Beispiel Erweiterung

![Screenshot Taskbar](https://raw.githubusercontent.com/patbec/TaskbarSampleExtension/master/screenshot-taskbar-sample-install.png)

In den Projektdateien gibt es den Ordner Test, dort liegen Batchdateien für die Installation / Deinstallation / Neuinstallation (Debugging).

Die Batch Dateien verwenden **gacutil** und **regasm** um die Erweiterung zu registrieren. Diese beiden Tools setzen _NETFX 4.6.1 Tools_ und _Framework64\v4.0.30319_ voraus. Ist Visual Studio 2017 installiert, sind diese Pakete bereits vorhanden.
Sollte eine andere .NET Framework Version installiert sein, einfach die Variable `path_gacutil=` und `path_regasm` anpassen.

Nach dem installieren wird die Erweiterung unter Symbolleisten angezeigt.

### Weiter Informationen

- Die TaskbarSampleExt Assembly ist per **Default** für COM-Komponenten **unsichtbar**, bei der eigenen Erweiterung das Attribute `[ComVisible(true)]` angeben.


- Kann die Erweiterung durch ein Problem etc. nicht mehr deinstalliert werden, den folgenden Registry Schlüssel mit der **entsprechenden GUID** entfernen.
  ```
  Computer\HKEY_CLASSES_ROOT\CLSID\{00000000-0000-0000-0000-000000000000}
  ```

  Anschließend die **Developer Command Prompt for VS 2017** Konsole starten.
  Mit dem Befehl `gacutil /l TaskbarSampleExt` wird im Cache nach der Assembly gesucht.
  Wenn ein Eintrag gefunden wurde, kann dieser durch den Befehl `gacutil /u TaskbarSampleExt` gelöscht werden.

## Autor

* **Patrick Becker** - [GitHub](https://github.com/patbec)

E-Mail: [github@bec-wolke.de](mailto:github@bec-wolke.de)

## Lizenz

Dieses Projekt ist unter der MIT-Lizenz lizenziert - Weitere Informationen finden Sie in der Datei [LICENSE](LICENSE)
