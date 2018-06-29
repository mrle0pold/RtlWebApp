import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import * as queryString from 'query-string'

interface FetchShowDataState {
    shows: Show[];
    loading: boolean;
    totalPages: number;
    page: number;
    currentpage: number;
}


export class FetchShowData extends React.Component<RouteComponentProps<{}>, FetchShowDataState> {
    constructor() {
        super();
        this.state = { shows: [], loading: true, totalPages: 0, page: 0,currentpage: 1 };
        this.fetchIt();
    }

    private fetchIt() {
        fetch('home/GetShows?page=' + this.state.currentpage)
            .then(response => response.json() as Promise<QueryResult>)
            .then(data => {

                this.setState({ shows: data.shows, loading: false });
                this.setState({ totalPages: data.totalPages, loading: false });
                this.setState({ page: data.page, loading: false });

            });
    }

    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderShowsTable(this.state.shows);

        return <div>
            <h1>Shows forecast</h1>
            <p>This component demonstrates fetching data from the server.</p>
            { contents }
        </div>;
    }

    private renderShowsTable(shows: Show[]) {
        return <div><h2>{this.renderPagination()}</h2>
            <table className='table'>
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

                    currentpage: {this.state.currentpage}
                </tbody>
            </table>
        </div>;
    }

    private renderPagination() {
        var rows = [];
        for (let i = 0; i < this.state.totalPages; i++) {
            rows.push(<a onClick={() => this.updatePage(i)}>{i} </a> );
        }
        return rows;

    }

    private updatePage(newpage: number)
    {
        this.setState({ currentpage: newpage });
        this.fetchIt();
    }
}


interface QueryResult {
    shows: Show[];
    totalPages: number;
    page: number;
}

interface Show {
    name: string;
    cast: CastMember[];
}

interface CastMember {
    name: string;
    birthDay: Date | null;
}
