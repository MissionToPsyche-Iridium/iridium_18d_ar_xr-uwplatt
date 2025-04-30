import { useCallback, useEffect, useState } from 'react';

type PerformanceType = {
  screenName: string; // name of the screen we're doing performace profiling on
  isLoading: boolean | undefined; // indicating if data loading is finished
};

export const useGetPerformanceReport = ({
  screenName,
  isLoading,
}: PerformanceType) => {
  const [startTime, setStartTime] = useState(0);
  const [loadTime, setLoadTime] = useState<string | null>(null); 

  // calculate wanted times and print report to console
  const getPerformanceReport = useCallback(() => { 

    // if load time hasn't been calculated before and no longer loading:
    if(loadTime === null && !isLoading)
    {
      //calculate load time and send to console
      const endTime = performance.now();
      const timeToRender = endTime - startTime;
      const logMessage = `${screenName} Load Time: ${timeToRender.toFixed(0)} milliseconds`;
      setLoadTime(logMessage);
      console.log(logMessage);
      
    }
    
  }, [screenName, startTime, isLoading, loadTime]);

  // two seperate useEffect() functions to prevent re-renders
  // on render, set the start time
  useEffect(() => {
    setStartTime(performance.now());
  }, []);

  // when isLoading is updated and no longer loading, generate report
  useEffect(() => {
    if(!isLoading) {
      getPerformanceReport();
    }
  }, [isLoading, getPerformanceReport]);

  // return performance times for use in other components
  return {
    loadTime,
  };
};