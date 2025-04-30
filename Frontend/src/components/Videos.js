import React from 'react';
import { useTranslation } from 'react-i18next'; // Import translation hook
import './Videos.css';
import Particles from './Particles'

const Videos = () => {
  const { t } = useTranslation(); // Hook to access translations

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
      <Particles />
      <h1 className="videos-title">{t('videos.title')}</h1>
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
