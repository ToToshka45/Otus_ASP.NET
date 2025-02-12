import { Component } from "react";
import PropTypes from "prop-types";

export class ClassBasedButton extends Component
{
    foo = () => { alert('alarm!'); }

    render()
    {

        const style = {
            color: "red",
            borderWidth: "2px",
            borderStyle: 'dotted',
        }

        const chislo = typeof this.props.chislo === 'function' ? this.props.chislo() : this.props.chislo;

        return <button style={style} onClick={this.foo}>
                    Old way button [{this.props.fancyText}] {chislo}
            </button>;
    }
}

ClassBasedButton.propTypes = {
    chislo: PropTypes.oneOfType([PropTypes.number, PropTypes.func]),
    fancyText: PropTypes.string.isRequired
}
