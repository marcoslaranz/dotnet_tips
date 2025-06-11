# Step-by-step guide to creating a VS Code Extension

## 🚀 Step 1: Install Prerequisites
Before starting, ensure you have:
- **Node.js** [(Download)](https://nodejs.org)
- **NPM** (comes with Node.js)
- **Yeoman & VS Code Generator**, install with:
  ```bash
  npm install -g yo generator-code
  ```

## ✅ Step 2: Generate a New Extension
Run:
```bash
yo code
```
📌 Follow the prompts:
- **Type**: Command, Webview, or Language Support
- **Name**: Pick a meaningful name
- **Package Details**: Provide a description

## 🔍 Step 3: Open & Explore the Extension
```bash
cd <your-extension-folder>
code .
```
### Key Files:
- `src/extension.ts` → Main entry point
- `package.json` → Defines metadata & commands
- `.vscode/` → Debug configs

## ⚙️ Step 4: Implement Features
Modify `src/extension.ts`:

```typescript
import * as vscode from 'vscode';

export function activate(context: vscode.ExtensionContext) {
    const disposable = vscode.commands.registerCommand('myExtension.helloWorld', () => {
        vscode.window.showInformationMessage('Hello from VS Code Extension!');
    });

    context.subscriptions.push(disposable);
}

export function deactivate() {}
```
📌 This registers a command in VS Code that shows a notification.

## 🛠️ Step 5: Test Your Extension
Run:
```bash
npm install
npm run compile
npm run test
```
### Start debugging:
```bash
F5
```
📌 Opens a new window with your extension loaded.

## 📦 Step 6: Publish Your Extension
1️⃣ **Install vsce** (VS Code Extensions CLI):
```bash
npm install -g vsce
```
2️⃣ **Build the extension package**:
```bash
vsce package
```
3️⃣ **Publish to the Marketplace**:
```bash
vsce publish
```
## Details in publishing your extension can be found here.

```bash
https://code.visualstudio.com/api/working-with-extensions/publishing-extension
```



## 🔍 Debugging & Testing
### 🚀 Step 1: Run the Extension in Debug Mode
```bash
cd <your-extension-folder>
code .
```
Press **F5** to launch.

### ✅ Step 2: Verify That Your Command Works
Run it from **Command Palette (Ctrl+Shift+P)**:
```
myExtension.helloWorld
```
📌 Check if it executes properly.

### 🔎 Step 3: Debug Issues
Open **Debug Console (Ctrl+Shift+Y)** and check for errors.
If needed, add:
```typescript
console.log('Extension is running!');
```

### 🚀 Step 4: Run Automated Tests
```bash
npm run test
```

## 📖 Additional Resources
Find more details in the [official VS Code extension docs](https://code.visualstudio.com/api).

---

