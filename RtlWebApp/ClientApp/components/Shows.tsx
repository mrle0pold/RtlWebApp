import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';

interface FetchShowDataState {
    
    shows: Show[];
    loading: boolean;

}

interface FetchShowDataProps {
    page: number;
}


export class FetchShowData extends React.Component<RouteComponentProps<FetchShowDataProps>, FetchShowDataState> {
    constructor() {
        super();
        this.state = { shows: [] , loading: true };

        fetch('home/GetShows?page=0')
            .then(response => response.json() as Promise<QueryResult>)
            .then(data => {
            
                this.setState({ shows: data.shows, loading: false });
                
            });
    }

    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchShowData.renderShowsTable(this.state.shows);

        return <div>
            <h1>Shows forecast</h1>
            <p>This component demonstrates fetching data from the server.</p>
            { contents }
        </div>;
    }

    private static renderShowsTable(shows: Show[]) {
        return <table className='table'>
            <thead>
                <tr>
                    <th>Naam</th>
                    <th>Cast</th>
                   </tr>
            </thead>
            <tbody>
                {shows.map(show =>
                    <tr key={show.name}>
                        <td>{show.name}</td>
                        {show.cast.map(cast =>
                            <tr><td>{cast.name}</td> <td>{cast.birthDay}</td>
                            </tr>
                        )}        
                    </tr>
                )}

              
            </tbody>
        </table>;
    }
}


interface QueryResult {
    shows: Show[];

}

interface Show {
    name: string;
    cast: CastMember[];
}

interface CastMember {
    name: string;
    birthDay: Date | null;
}
