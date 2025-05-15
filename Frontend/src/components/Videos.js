/**
 * Videos.js
 *
 * Author(s): Sam Miller, Lahiru Suraweera
 * Purpose: This React component displays a set of embedded YouTube videos,
 *          each with a translated title. It includes particle animation for visual effect
 *          and uses i18n for multilingual support.
 * Date Written: May 14, 2025
 */

import React from 'react';
import { useTranslation } from 'react-i18next'; // Import translation hook for i18n
import './Videos.css'; // CSS styling for the video layout
import Particles from './Particles'; // Optional animated background component

/**
 * Videos Component
 *
 * Renders a list of educational or promotional YouTube videos using <iframe> embeds.
 * Each video includes a translated title and is displayed in a grid layout.
 */
const Videos = () => {
  const { t } = useTranslation(); // Hook to access translation function

  // Array of video data including title (from translation) and YouTube embed URL
  const videos = [
    {
      id: 1,
      title: t('videos.video1.title'),
      src: 'https://www.youtube.com/embed/51_L6DpoPzE?si=D3I8mDHdBfC5GFzV',
    },
    {
      id: 2,
      title: t('videos.video2.title'),
      src: 'https://www.youtube.com/embed/gCmIZ_sZbEM?si=q0XB5q1B3_7EzN66',
    },
    {
      id: 3,
      title: t('videos.video3.title'),
      src: 'https://www.youtube.com/embed/AwCiHscmEQE?si=SDThWfrQiGT2hAyh',
    },
    {
      id: 4,
      title: t('videos.video4.title'),
      src: 'https://www.youtube.com/embed/HhjfryP25lk?si=qrLUocweOFCPSdcs',
    },
    {
      id: 5,
      title: t('videos.video5.title'),
      src: 'https://www.youtube.com/embed/g1pnv6tQSJ0?si=YB-MR7GZmhcuOj86',
    },
  ];

  return (
    <div className="videos-container">
      <Particles /> {/* Animated background effect */}
      <h1 className="videos-title">{t('videos.title')}</h1>

      {/* Grid layout for all videos */}
      <div className="videos-grid">
        {videos.map((video) => (
          <div key={video.id} className="video-card">
            <h2>{video.title}</h2>
            <iframe
              src={video.src}
              width="560"
              height="315"
              title={video.title}
              frameBorder="0"
              allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
              allowFullScreen
              referrerPolicy="strict-origin-when-cross-origin"
            ></iframe>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Videos;
