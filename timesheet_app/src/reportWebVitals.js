import { report } from 'web-vitals';

const reportWebVitals = (onPerfEntry) => {
    if (onPerfEntry && onPerfEntry instanceof Function) {
        report(onPerfEntry);
    }
};

export default reportWebVitals;
