/**
 * Credits.js
 *
 * Author(s): Sam Miller, Lahiru Suraweera
 * Purpose: This React component displays the Credits section of the application,
 *          providing acknowledgment to contributors and supporters, styled with
 *          a background, particles effect, and internationalized text.
 * Date Written: May 14, 2025
 */

import React from 'react';
import { useTranslation } from 'react-i18next'; // Hook for i18n translations
import './Credits.css'; // Styles specific to the credits section
import Particles from './Particles'; // Visual effect component

/**
 * Credits Component
 * 
 * Renders a styled credits page that includes:
 * - Background visuals
 * - A particle animation layer
 * - A translated list of project contributors and acknowledgments
 * - Footer with additional information and disclaimers
 */
const Credits = () => {
  const { t } = useTranslation(); // Access the translation function

  return (
    <div>
      {/* Background overlay */}
      <div className="credits-background"></div>

      {/* Main container for credits content */}
      <div id="credits" className="credits-container">
        <Particles /> {/* Animated background particles */}

        {/* Credits page header */}
        <h1 className="credits-title">{t('credits.title')}</h1>
        <p className="credits-description">{t('credits.description')}</p>

        {/* List of contributors */}
        <ul className="credits-list">
          <li><strong>{t('credits.projectLead')}</strong></li>

          <li>
            <strong>{t('credits.frontendTeam')}:</strong>{' '}
            {t('credits.frontendMembers', 'Lahiru Suraweera, Sam Miller, Samuel Bergemann, Zach Burrell, Eli Jacobson')}
          </li>

          <li>
            <strong>{t('credits.arTeam')}:</strong>{' '}
            {t('credits.arMembers', 'Thomas Apel, Alexander Cleaver, Jack Linke, Evan Lee, Kase Tadych, Andrew Suetholz, Logan Lusk, April Woolcock')}
          </li>

          <li>
            <strong>{t('credits.media')}:</strong>{' '}
            {t('credits.nasaTeam')}
          </li>

          <li>
            <strong>{t('credits.specialThanks')}:</strong>{' '}
            {t('credits.specialThanksNames', 'Dr. Cassie Bowman and the Psyche Mission Team at ASU')}
          </li>
        </ul>

        {/* Footer with link to official website */}
        <p className="credits-footer">
          {t('credits.moreInfo')}{' '}
          <a href="https://psyche.asu.edu" target="_blank" rel="noopener noreferrer">
            {t('credits.website')}
          </a>.
        </p>

        {/* Disclaimer text */}
        <p className="credits-disclaimer">
          <strong>{t('credits.disclaimerLabel')} </strong>
          {t('credits.disclaimerText')}
        </p>
      </div>
    </div>
  );
};

export default Credits;
