/**
 * i18n.js
 *
 * Author(s): Sam Miller, Lahiru Suraweera
 * Purpose: Initializes and configures the i18next internationalization library
 *          for React. Supports multiple languages and stores user preference in localStorage.
 * Date Written: May 14, 2025
 */

import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

// Import translation files for supported languages
import en from './locales/en.json';
import es from './locales/es.json';
import fr from './locales/fr.json';
import md from './locales/md.json'; // Mandarin/Chinese translations

// Initialize i18next with configuration settings
i18n
  .use(initReactI18next) // Connects i18n with React
  .init({
    resources: {
      en: { translation: en }, // English translations
      es: { translation: es }, // Spanish translations
      fr: { translation: fr }, // French translations
      md: { translation: md }, // Chinese (Mandarin) translations
    },
    lng: localStorage.getItem('appLanguage') || 'en', // Default to saved language or English
    fallbackLng: 'en', // Use English if translation is missing
    interpolation: {
      escapeValue: false, // React already escapes content, so this should be false
    },
  });

export default i18n;
