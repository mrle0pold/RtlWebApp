import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Pagination } from './Pagination';
import { FetchShowData } from './Shows';


export class Home extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return <div>
            <h1>Hello, home</h1>
           
        </div>;
    }
}
