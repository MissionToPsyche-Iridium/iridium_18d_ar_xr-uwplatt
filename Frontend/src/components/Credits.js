import React from 'react';
import { useTranslation } from 'react-i18next'; // Import translation hook
import './Credits.css';

const Credits = () => {
  const { t } = useTranslation(); // Hook to access translations

  return (
    <div>
      <div className="credits-background"></div>
      <div id="credits" className="credits-container">
        <h1 className="credits-title">{t('credits.title')}</h1>
        <p className="credits-description">{t('credits.description')}</p>
        <ul className="credits-list">
          <li><strong>{t('credits.projectLead')}</strong></li>
          <li><strong>{t('credits.frontendTeam')}:</strong> Lahiru Suraweera, Sam Miller, Samuel Bergemann, Zach Burrell, Eli Jacobson</li>
          <li><strong>{t('credits.arTeam')}:</strong> Thomas Apel, Alexander Cleaver, Jack Linke, Evan Lee, Kase Tadych, Andrew Suetholz, Logan Lusk, April Woolcock</li>
          <li><strong>{t('credits.media')}:</strong> {t('credits.nasaTeam')}</li>
          <li><strong>{t('credits.specialThanks')}:</strong> Dr. Cassie Bowman and the Psyche Mission Team at ASU</li>
          <li><strong>{t('credits.disclaimer')}:</strong> Disclaimer": </li>
        </ul>
        <p className="credits-footer">
          {t('credits.moreInfo')} <a href="https://psyche.asu.edu" target="_blank" rel="noopener noreferrer">{t('credits.website')}</a>.
        </p>
        <p className="credits-disclaimer">
        <strong>{t('Disclaimer: ')}</strong>This work was created in partial fulfillment of University of Wisconsin – Platteville Capstone Course SE 4730. The work is a result of the Psyche Student Collaborations component of NASA’s Psyche Mission (https://psyche.asu.edu/). “Psyche: A Journey to a Metal World” [Contract number NNM16AA09C] is part of the NASA Discovery Program mission to solar system targets. Trade names and trademarks of ASU and NASA are used in this work for identification only. Their usage does not constitute an official endorsement, either expressed or implied, by Arizona State University or National Aeronautics and Space Administration. The content is solely the responsibility of the authors and does not necessarily represent the official views of ASU or NASA.​
        </p>
      </div>
    </div>
  );
};

export default Credits;
