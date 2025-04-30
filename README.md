# Psyche AR/VR Web Experience

A React-based, multi-language, AR/VR demo site for the NASA Psyche mission.  
Users can explore an augmented-reality scene, learn about Psyche, watch related videos, and read project credits—all wrapped in an interactive starfield background.

---


## 🚀 Features

- **AR Mode**  
  Embedded Unity build via `<iframe>`, with camera & microphone permissions.  
- **Learn More**  
  Interactive cards with deep-dive content and imagery.  
- **Instructions**  
  Step-by-step guide on how to launch and use AR features.  
- **Videos**  
  Curated Psyche mission clips.  
- **Credits**  
  Project team acknowledgements.  
- **Multi-language Support**  
  English, Español, Français, 中文 via `react-i18next`.  
- **Dynamic Starfield**  
  Nebula & particles background with `@flodlc/nebula` and `react-tsparticles`.  
- **Responsive Design**  
  Built with React-Bootstrap for mobile-first layouts.

---

## 🛠️ Tech Stack

- **Framework:** React 18  
- **Styling/UI:** React-Bootstrap, custom CSS  
- **i18n:** `i18next` + `react-i18next`  
- **Particles:** `react-tsparticles` (slim build)  
- **Nebula:** `@flodlc/nebula`  
- **CI/CD & Pages:** GitLab CI on `node:18` image → GitLab Pages  
- **Bundler:** Create React App (Webpack)

---

## 📥 Installation & Local Development

1. **Clone & enter frontend folder**  
   ```bash
   git clone <your-repo-url>
   cd <repo>/Frontend
   npm install typescript@4.9.5 --save-dev --legacy-peer-deps
   npm ci --legacy-peer-deps
   npm install i18next react-i18next --legacy-peer-deps
   npm install react-tsparticles tsparticles --legacy-peer-deps
   npm install tsparticles-slim --legacy-peer-deps
   npm install @flodlc/nebula --legacy-peer-deps


## 📝 Translations
All user-facing strings live under your public/locales/{en,es,fr,md}/… JSON files.
To add/update text, edit those JSON keys (e.g. instructions.step1.title) then rebuild.


## 🤝 Contributing
1. Fork the repo.

2. Create a feature branch: git checkout -b feature/YourFeature.

3. Commit your changes, run tests, ensure lint passes.

4. Push and open a Merge Request against dev.